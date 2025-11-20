namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IGetProfilesHandler
    {
        Task<IResult> HandleAsync(HttpContext context, Guid id, CancellationToken token);
    }
}