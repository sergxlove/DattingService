namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IProfileInterestHandler
    {
        Task<IResult> HandleAsync(HttpContext context, Guid id, CancellationToken token);
    }
}