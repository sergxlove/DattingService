using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class GetProfilesHandler : IGetProfilesHandler
    {
        private readonly IUsersService _userService;

        public GetProfilesHandler(IUsersService userService)
        {
            _userService = userService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, Guid id, CancellationToken token)
        {
            try
            {
                Users? result = await _userService.GetByIdAsync(id, token);
                if (result is null) return Results.BadRequest("error");
                return Results.Ok(result);
            }
            catch
            {
                return Results.BadRequest("error");
            }
        }
    }
}
