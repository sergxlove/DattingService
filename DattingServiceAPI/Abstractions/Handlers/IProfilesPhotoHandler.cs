namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IProfilesPhotoHandler
    {
        Task<IResult> HandleAsync(HttpContext context, string photoName, CancellationToken token);
    }
}