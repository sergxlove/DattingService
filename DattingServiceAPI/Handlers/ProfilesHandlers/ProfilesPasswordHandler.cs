using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class ProfilesPasswordHandler : IProfilesPasswordHandler
    {
        private readonly ILoginUsersService _loginService;

        public ProfilesPasswordHandler(ILoginUsersService loginService)
        {
            _loginService = loginService;
        }

        public async Task<IResult> Handle(HttpContext context, string newPassword,
            CancellationToken token)
        {
            try
            {
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                string email = await _loginService.GetEmailAsync(id, token);
                Result<LoginUsers> userLogin = LoginUsers.Create(id, email, newPassword);
                if (!userLogin.IsSuccess) return Results.BadRequest(userLogin.Error);
                await _loginService.UpdatePasswordAsync(userLogin.Value, token);
                return Results.Ok();
            }
            catch
            {
                return Results.BadRequest("error");
            }
        }
    }
}
