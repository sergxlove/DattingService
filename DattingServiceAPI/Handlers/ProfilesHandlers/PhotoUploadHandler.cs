using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class PhotoUploadHandler : IPhotoUploadHandler
    {
        private readonly IPhotoMovedService _photoMovedService;

        public PhotoUploadHandler(IPhotoMovedService photoMovedService)
        {
            _photoMovedService = photoMovedService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, IFormFile file,
            CancellationToken token)
        {
            try
            {
                if (file is null || file.Length == 0)
                    return Results.BadRequest("No file uploaded");
                if (file.Length > 10 * 1024 * 1024)
                    return Results.BadRequest("File too large (max 10MB)");
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                using Stream stream = file.OpenReadStream();
                bool result = await _photoMovedService.AddPhotoAsync(stream, id,
                    file.FileName, token);
                if (!result) return Results.BadRequest("photo no add");
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex);
            }
        }
    }

}
