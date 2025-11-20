using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class ProfileInterestHandler : IProfileInterestHandler
    {
        private readonly IInterestsService _interestService;

        public ProfileInterestHandler(IInterestsService interestsService)
        {
            _interestService = interestsService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, Guid id, CancellationToken token)
        {
            int[] result = await _interestService.GetAsync(id, token);
            return Results.Ok(result);
        }
    }
}
