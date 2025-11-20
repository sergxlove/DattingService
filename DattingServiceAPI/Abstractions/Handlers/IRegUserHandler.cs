using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IRegUserHandler
    {
        Task<IResult> HandleAsync(HttpContext context, RegistrRequest request, CancellationToken token);
    }
}