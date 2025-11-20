using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface ILoginUserHandler
    {
        Task<IResult> HandleAsync(HttpContext context, LoginRequest request, CancellationToken token);
    }
}