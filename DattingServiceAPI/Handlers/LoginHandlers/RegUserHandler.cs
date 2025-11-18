using DattingService.Core.Models;
using Microsoft.AspNetCore.Mvc;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.LoginHandlers
{
    public class RegUserHandler
    {

        private readonly IRegistrUserService _registrService;
        private readonly IJwtProviderService _jwtService;
        private readonly IConfiguration _config;

        public RegUserHandler(IRegistrUserService registrService, IJwtProviderService jwtService, 
            IConfiguration config)
        {
            _registrService = registrService;
            _jwtService = jwtService;
            _config = config;
        }

        public async Task<IResult> HandleAsync(HttpContext context, RegistrRequest request,
            CancellationToken token)
        {
            try
            {
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                string roleUser = "user";
                if (idStr == string.Empty || roleUser == string.Empty)
                    return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                int lifetimeAccess = Convert.ToInt32(_config["JwtSettings:LifetimeAccessMinutes"]);
                int lifetimeRefresh = Convert.ToInt32(_config["JwtSettings:LifetimeRefreshDays"]);
                Result<Users> usersResult = Users.Create(id, request.Name, request.Age, request.Target,
                    request.Description, request.City, true, true);
                if (!usersResult.IsSuccess) return Results.BadRequest(usersResult.Error);
                Result<TokensUser> tokensUser = TokensUser.Create(Guid.NewGuid(), usersResult.Value.Id,
                    DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(lifetimeRefresh), roleUser!);
                if (!tokensUser.IsSuccess) return Results.BadRequest(tokensUser.Error);
                bool result = await _registrService.RegistrationAsync(usersResult.Value,
                    tokensUser.Value, token);
                if (!result) return Results.BadRequest("no reg");
                var accessClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Sid, idStr!),
                        new Claim(ClaimTypes.Role, roleUser!)
                    };
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
                cookieOptions.MaxAge = TimeSpan.FromDays(lifetimeRefresh);
                context.Response.Cookies.Append("refresh_token", tokensUser.Value.Id.ToString(),
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
