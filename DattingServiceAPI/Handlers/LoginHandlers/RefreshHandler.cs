using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.LoginHandlers
{
    public class RefreshHandler : IRefreshHandler
    {

        private readonly ITokensUserService _tokenService;
        private readonly IJwtProviderService _jwtService;
        private readonly IConfiguration _config;

        public RefreshHandler(ITokensUserService tokenService, IJwtProviderService jwtService,
            IConfiguration config)
        {
            _tokenService = tokenService;
            _jwtService = jwtService;
            _config = config;
        }

        public async Task<IResult> HandleAsync(HttpContext context, CancellationToken token)
        {
            string? refreshToken = context.Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Results.Unauthorized();
            Guid refreshTokenGuid = Guid.Parse(refreshToken!);
            TokensUser? tokenDb = await _tokenService.GetAsync(refreshTokenGuid, token);
            if (tokenDb is null) return Results.Unauthorized();
            if (tokenDb is null) return Results.Unauthorized();
            var accessClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, tokenDb.UserId.ToString()),
                    new Claim(ClaimTypes.Role, tokenDb.Role)
                };
            int lifetimeAccess = Convert.ToInt32(_config["JwtSettings:LifetimeAccessMinutes"]);
            int lifetimeRefresh = Convert.ToInt32(_config["JwtSettings:LifetimeRefreshDays"]);
            string accessToken = _jwtService.GenerateToken(new JwtRequest
            {
                Audience = _config["JwtSettings:Audiens"]!,
                Issuer = _config["JwtSettings:Issuer"]!,
                Claims = accessClaims,
                Expires = DateTime.UtcNow.AddMinutes(lifetimeAccess),
                SecretKey = _config["JwtSettings:SecretKey"]!
            });
            Result<TokensUser> newTokenUser = TokensUser.Create(Guid.NewGuid(), tokenDb.UserId,
                    DateTime.UtcNow, DateTime.UtcNow + TimeSpan.FromDays(lifetimeRefresh),
                    tokenDb.Role);
            if (!string.IsNullOrEmpty(newTokenUser.Error))
                return Results.BadRequest(newTokenUser.Error);
            int result = await _tokenService.UpdateAsync(newTokenUser.Value, token);
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
        }
    }
}
