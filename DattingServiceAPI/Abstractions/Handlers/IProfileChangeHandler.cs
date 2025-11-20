using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IProfileChangeHandler
    {
        Task<IResult> HandleAsync(HttpContext context, RegistrRequest request, CancellationToken token);
    }
}