using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.LoginHandlers
{
    public class RegLoginUserHandler : IRegLoginUserHandler
    {

        private readonly IJwtProviderService _jwtService;
        private readonly ITempLoginUsersService _tempLoginUsersService;
        private readonly ILoginUsersService _loginUserService;
        private readonly IConfiguration _config;

        public RegLoginUserHandler(IJwtProviderService jwtService, ITempLoginUsersService tempLoginUsersService,
            ILoginUsersService loginUsersService, IConfiguration config)
        {
            _jwtService = jwtService;
            _tempLoginUsersService = tempLoginUsersService;
            _loginUserService = loginUsersService;
            _config = config;
        }

        public async Task<IResult> HandleAsync(HttpContext context, RegLoginRequest request,
            CancellationToken token)
        {
            try
            {
                if (request is null) return Results.BadRequest("request empty");
                if (await _loginUserService.CheckAsync(request.Username, token))
                    return Results.BadRequest("email busy");
                if (await _tempLoginUsersService.CheckAsync(request.Username, token))
                    return Results.BadRequest("email busy");
                if (request.Username == string.Empty || request.Password == string.Empty
                    || request.AgainPassword == string.Empty)
                {
                    return Results.BadRequest("login or password is empty");
                }
                Result<LoginUsers> user = LoginUsers.Create(request.Username, request.Password);
                if (!user.IsSuccess) return Results.BadRequest(user.Error);
                await _tempLoginUsersService.AddAsync(user.Value, token);
                List<Claim> accessClaims = new()
                    {
                        new Claim(ClaimTypes.Role, "beginRegUser"),
                        new Claim(ClaimTypes.Sid, user.Value.Id.ToString()!),
                    };
                int lifetimeAccess = Convert.ToInt32(_config["JwtSettings:LifetimeAccessMinutes"]);
                string accessToken = _jwtService.GenerateToken(new JwtRequest
                {
                    Audience = _config["JwtSettings:Audiens"]!,
                    Issuer = _config["JwtSettings:Issuer"]!,
                    Claims = accessClaims,
                    Expires = DateTime.UtcNow.AddMinutes(lifetimeAccess),
                    SecretKey = _config["JwtSettings:SecretKey"]!
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
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest("error");
            }
        }
    }
}
