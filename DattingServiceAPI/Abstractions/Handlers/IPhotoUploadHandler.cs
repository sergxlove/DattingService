namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IPhotoUploadHandler
    {
        Task<IResult> HandleAsync(HttpContext context, IFormFile file, CancellationToken token);
    }
}