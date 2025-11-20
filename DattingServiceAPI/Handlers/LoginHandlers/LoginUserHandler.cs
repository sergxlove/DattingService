using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.LoginHandlers
{
    public class LoginUserHandler : ILoginUserHandler
    {

        private readonly ILoginUsersService _loginUserService;
        private readonly IJwtProviderService _jwtGenerate;
        private readonly ITokensUserService _tokenService;
        private readonly IConfiguration _config;

        public LoginUserHandler(ILoginUsersService loginUsersService, IJwtProviderService jwtGenerate,
            ITokensUserService tokensUserService, IConfiguration config)
        {
            _loginUserService = loginUsersService;
            _jwtGenerate = jwtGenerate;
            _tokenService = tokensUserService;
            _config = config;
        }

        public async Task<IResult> HandleAsync(HttpContext context, LoginRequest request,
            CancellationToken token)
        {
            try
            {
                if (request.Username == string.Empty || request.Password == string.Empty)
                    return Results.BadRequest("login or password is empty");

                if (!await _loginUserService!.CheckAsync(request.Username, token))
                    return Results.BadRequest("user is not found");

                Guid idUser = await _loginUserService.VerifyAsync(request.Username, request.Password, token);
                if (idUser == Guid.Empty) return Results.BadRequest("no auth");
                string roleUser = "user";
                List<Claim> accessClaims = new()
                    {
                        new Claim(ClaimTypes.Role, roleUser),
                        new Claim(ClaimTypes.Sid, idUser.ToString()!),
                    };
                int lifetimeAccess = Convert.ToInt32(_config["JwtSettings:LifetimeAccessMinutes"]);
                int lifetimeRefresh = Convert.ToInt32(_config["JwtSettings:LifetimeRefreshDays"]);
                string accessToken = _jwtGenerate!.GenerateToken(new JwtRequest()
                {
                    Audience = _config["JwtSettings:Audiens"]!,
                    Issuer = _config["JwtSettings:Issuer"]!,
                    Claims = accessClaims,
                    Expires = DateTime.UtcNow.AddMinutes(lifetimeAccess),
                    SecretKey = _config["JwtSettings:SecretKey"]!
                });
                Result<TokensUser> tokenUser = TokensUser.Create(Guid.NewGuid(), idUser,
                    DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(lifetimeRefresh), roleUser);
                if (!string.IsNullOrEmpty(tokenUser.Error))
                    return Results.BadRequest(tokenUser.Error);
                int result = await _tokenService.UpdateAsync(tokenUser.Value, token);
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
        }
    }
}
