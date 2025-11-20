using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IRegLoginUserHandler
    {
        Task<IResult> HandleAsync(HttpContext context, RegLoginRequest request, CancellationToken token);
    }
}