using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IInterestsUpdateHandler
    {
        Task<IResult> HandleAsync(HttpContext context, InterestsRequest request, CancellationToken token);
    }
}