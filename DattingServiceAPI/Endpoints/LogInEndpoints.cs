using DattingService.Core.Models;
using Microsoft.AspNetCore.Mvc;
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
                context.Response.Cookies.Delete("jwt");
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/api/users/login", async (HttpContext context,
                [FromBody] LoginRequest request,
                [FromServices] ILoginUsersService loginUserService,
                [FromServices] IJwtProviderService jwtGenerate,
                CancellationToken token) =>
            {
                try
                {
                    if (request.Username == string.Empty || request.Password == string.Empty)
                        return Results.BadRequest("login or password is empty");
                    
                    if (!await loginUserService!.CheckAsync(request.Username, token))
                       return Results.BadRequest("user is not found");

                    Guid? idUser = await loginUserService.VerifyAsync(request.Username, request.Password, token);
                    if (idUser == null) return Results.BadRequest("no auth");

                    List<Claim> claims = new()
                    {
                        new Claim(ClaimTypes.Role, "user"),
                        new Claim(ClaimTypes.Sid, idUser.ToString()!),
                    };
                    string? jwttoken = jwtGenerate!.GenerateToken(new JwtRequest()
                    {
                        Claims = claims
                    });
                    context.Response.Cookies.Append("jwt", jwttoken!);
                    return Results.Ok(token!);
                }
                catch
                {
                    return Results.BadRequest("error");
                }
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
                    if(await tempLoginUsersService.CheckAsync(request.Username, token))
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
                CancellationToken token) =>
            {
                try
                {
                    string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    if (idStr == string.Empty) return Results.BadRequest("error");
                    Guid id = Guid.Parse(idStr!);
                    Result<Users> usersResult = Users.Create(id, request.Name, request.Age, request.Target,
                        request.Description, request.City, true, true);
                    if (!usersResult.IsSuccess) return Results.BadRequest(usersResult.Error);
                    bool result = await registrService.RegistrationAsync(usersResult.Value, token);
                    if(result) return Results.Ok();
                    return Results.BadRequest("no reg");
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapDelete("/api/users/delete", (HttpContext context) =>
            {
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);

                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
