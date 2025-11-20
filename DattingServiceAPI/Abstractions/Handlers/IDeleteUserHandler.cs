namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IDeleteUserHandler
    {
        Task<IResult> HandleAsync(HttpContext context, CancellationToken token);
    }
}