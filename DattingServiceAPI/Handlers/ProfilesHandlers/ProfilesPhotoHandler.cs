using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class ProfilesPhotoHandler : IProfilesPhotoHandler
    {
        private readonly IPhotosService _photosService;

        public ProfilesPhotoHandler(IPhotosService photosService)
        {
            _photosService = photosService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, string photoName,
            CancellationToken token)
        {
            Stream? stream = await _photosService.DownloadFromNameAsync(photoName, "photopr", token);
            if (stream is null) return Results.BadRequest();
            return Results.Stream(stream);
        }
    }
}
