namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IRefreshHandler
    {
        Task<IResult> HandleAsync(HttpContext context, CancellationToken token);
    }
}