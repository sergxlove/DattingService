namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IPhotoDeleteHandler
    {
        Task<IResult> HandleAsync(HttpContext context, CancellationToken token);
    }
}