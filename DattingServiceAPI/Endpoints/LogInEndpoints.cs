using DattingService.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Endpoints
{
    public static class LogInEndpoints
    {
        public static IEndpointRouteBuilder MapLogInEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/logout", (HttpContext context) =>
            {
                context.Response.Cookies.Delete("access_token");
                context.Response.Cookies.Delete("refresh_token");
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/api/users/login", async (HttpContext context,
                [FromBody] LoginRequest request,
                [FromServices] ILoginUsersService loginUserService,
                [FromServices] IJwtProviderService jwtGenerate,
                [FromServices] ITokensUserService tokenService,
                IConfiguration config,
                CancellationToken token) =>
            {
                try
                {
                    if (request.Username == string.Empty || request.Password == string.Empty)
                        return Results.BadRequest("login or password is empty");

                    if (!await loginUserService!.CheckAsync(request.Username, token))
                        return Results.BadRequest("user is not found");

                    Guid idUser = await loginUserService.VerifyAsync(request.Username, request.Password, token);
                    if (idUser == Guid.Empty) return Results.BadRequest("no auth");
                    string roleUser = "user";
                    List<Claim> accessClaims = new()
                    {
                        new Claim(ClaimTypes.Role, roleUser),
                        new Claim(ClaimTypes.Sid, idUser.ToString()!),
                    };
                    int lifetimeAccess = Convert.ToInt32(config["JwtSettings:LifetimeAccessMinutes"]);
                    int lifetimeRefresh = Convert.ToInt32(config["JwtSettings:LifetimeRefreshDays"]);
                    string accessToken = jwtGenerate!.GenerateToken(new JwtRequest()
                    {
                        Audience = config["Jwt:Audience"]!,
                        Issuer = config["Jwt:Issuer"]!,
                        Claims = accessClaims,
                        Expires = DateTime.UtcNow.AddMinutes(lifetimeAccess),
                        SecretKey = config["Jwt:Key"]!
                    });
                    Result<TokensUser> tokenUser = TokensUser.Create(Guid.NewGuid(), idUser,
                        DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(lifetimeRefresh), roleUser);
                    if (!string.IsNullOrEmpty(tokenUser.Error))
                        return Results.BadRequest(tokenUser.Error);
                    int result = await tokenService.UpdateAsync(tokenUser.Value, token);
                    if (result == 0) return Results.Unauthorized();
                    CookieOptions cookieOptions = new()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        IsEssential = true
                    };
                    cookieOptions.MaxAge = TimeSpan.FromMinutes(lifetimeAccess);
                    context.Response.Cookies.Append("access_token", accessToken, cookieOptions);
                    cookieOptions.MaxAge = TimeSpan.FromDays(lifetimeRefresh);
                    context.Response.Cookies.Append("refresh_token", tokenUser.Value.Id.ToString(),
                        cookieOptions);
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapPost("/api/refresh", async (HttpContext context,
                [FromServices] ITokensUserService tokenService,
                [FromServices] IJwtProviderService jwtService,
                IConfiguration config,
                CancellationToken token) =>
            {
                string? refreshToken = context.Request.Cookies["refresh_token"];
                if (string.IsNullOrEmpty(refreshToken))
                    return Results.Unauthorized();
                Guid refreshTokenGuid = Guid.Parse(refreshToken!);
                TokensUser? tokenDb = await tokenService.GetAsync(refreshTokenGuid, token);
                if (tokenDb is null) return Results.Unauthorized();
                if (tokenDb is null) return Results.Unauthorized();
                var accessClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, tokenDb.UserId.ToString()),
                    new Claim(ClaimTypes.Role, tokenDb.Role)
                };
                int lifetimeAccess = Convert.ToInt32(config["JwtSettings:LifetimeAccessMinutes"]);
                int lifetimeRefresh = Convert.ToInt32(config["JwtSettings:LifetimeRefreshDays"]);
                string accessToken = jwtService.GenerateToken(new JwtRequest
                {
                    Audience = config["Jwt:Audience"]!,
                    Issuer = config["Jwt:Issuer"]!,
                    Claims = accessClaims,
                    Expires = DateTime.UtcNow.AddMinutes(lifetimeAccess),
                    SecretKey = config["Jwt:Key"]!
                });
                Result<TokensUser> newTokenUser = TokensUser.Create(Guid.NewGuid(), tokenDb.UserId,
                        DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(lifetimeRefresh),
                        tokenDb.Role);
                if (!string.IsNullOrEmpty(newTokenUser.Error))
                    return Results.BadRequest(newTokenUser.Error);
                int result = await tokenService.UpdateAsync(newTokenUser.Value, token);
                if (result == 0) return Results.Unauthorized();
                CookieOptions cookieOptions = new()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    IsEssential = true
                };
                cookieOptions.MaxAge = TimeSpan.FromMinutes(lifetimeAccess);
                context.Response.Cookies.Append("access_token", accessToken, cookieOptions);
                cookieOptions.MaxAge = TimeSpan.FromDays(lifetimeRefresh);
                context.Response.Cookies.Append("refresh_token", tokenDb.Id.ToString(),
                    cookieOptions);
                return Results.Ok();
            });

            app.MapPost("/api/users/regLoginUser", async (HttpContext context,
                [FromBody] RegLoginRequest request,
                [FromServices] IJwtProviderService jwtGenerate,
                [FromServices] ITempLoginUsersService tempLoginUsersService,
                [FromServices] ILoginUsersService loginUserService,
                CancellationToken token) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("request empty");
                    if (await loginUserService.CheckAsync(request.Username, token))
                        return Results.BadRequest("email busy");
                    if (await tempLoginUsersService.CheckAsync(request.Username, token))
                        return Results.BadRequest("email busy");
                    if (request.Username == string.Empty || request.Password == string.Empty
                        || request.AgainPassword == string.Empty)
                    {
                        return Results.BadRequest("login or password is empty");
                    }
                    Result<LoginUsers> user = LoginUsers.Create(request.Username, request.Password);
                    if (!user.IsSuccess) return Results.BadRequest(user.Error);
                    await tempLoginUsersService.AddAsync(user.Value, token);
                    List<Claim> claims = new()
                    {
                        new Claim(ClaimTypes.Role, "user"),
                        new Claim(ClaimTypes.Sid, user.Value.Id.ToString()!),
                    };
                    string? jwttoken = jwtGenerate!.GenerateToken(new JwtRequest()
                    {
                        Claims = claims
                    });
                    context.Response.Cookies.Append("jwt", jwttoken!);
                    return Results.Ok(jwttoken!);
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapPost("/api/users/regUser", async (HttpContext context,
                [FromBody] RegistrRequest request,
                [FromServices] IRegistrUserService registrService,
                [FromServices] IJwtProviderService jwtService,
                IConfiguration config,
                CancellationToken token) =>
            {
                try
                {
                    string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    string? roleUser = context.User.FindFirst(ClaimTypes.Role)?.Value;
                    if (idStr == string.Empty || roleUser == string.Empty)
                        return Results.BadRequest("error");
                    Guid id = Guid.Parse(idStr!);
                    int lifetimeAccess = Convert.ToInt32(config["JwtSettings:LifetimeAccessMinutes"]);
                    int lifetimeRefresh = Convert.ToInt32(config["JwtSettings:LifetimeRefreshDays"]);
                    Result<Users> usersResult = Users.Create(id, request.Name, request.Age, request.Target,
                        request.Description, request.City, true, true);
                    if (!usersResult.IsSuccess) return Results.BadRequest(usersResult.Error);
                    Result<TokensUser> tokensUser = TokensUser.Create(Guid.NewGuid(), usersResult.Value.Id,
                        DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(lifetimeRefresh), roleUser!);
                    if (!tokensUser.IsSuccess) return Results.BadRequest(tokensUser.Error);
                    bool result = await registrService.RegistrationAsync(usersResult.Value,
                        tokensUser.Value, token);
                    if (!result) return Results.BadRequest("no reg");
                    var accessClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, idStr!),
                        new Claim(ClaimTypes.Role, roleUser!)
                    };
                    string accessToken = jwtService.GenerateToken(new JwtRequest
                    {
                        Audience = config["Jwt:Audience"]!,
                        Issuer = config["Jwt:Issuer"]!,
                        Claims = accessClaims,
                        Expires = DateTime.UtcNow.AddMinutes(lifetimeAccess),
                        SecretKey = config["Jwt:Key"]!
                    });
                    CookieOptions cookieOptions = new()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        IsEssential = true
                    };
                    cookieOptions.MaxAge = TimeSpan.FromMinutes(lifetimeAccess);
                    context.Response.Cookies.Append("access_token", accessToken, cookieOptions);
                    cookieOptions.MaxAge = TimeSpan.FromDays(lifetimeRefresh);
                    context.Response.Cookies.Append("refresh_token", tokensUser.Value.Id.ToString(),
                        cookieOptions);
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapDelete("/api/users/delete", async (HttpContext context,
                [FromServices] IRegistrUserService registrService,
                CancellationToken token) =>
            {
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                bool result = await registrService.DeleteUserAsync(id, token);
                if (!result) return Results.BadRequest("no delete");
                context.Response.Cookies.Delete("jwt");
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
