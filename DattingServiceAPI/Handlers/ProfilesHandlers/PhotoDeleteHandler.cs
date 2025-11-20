using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class PhotoDeleteHandler : IPhotoDeleteHandler
    {
        private readonly IPhotoMovedService _photoMovedService;

        public PhotoDeleteHandler(IPhotoMovedService photoMovedService)
        {
            _photoMovedService = photoMovedService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, CancellationToken token)
        {
            try
            {
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                bool result = await _photoMovedService.DeletePhotoAsync(id, token);
                if (!result) Results.BadRequest("photo no delete");
                return Results.Ok();
            }
            catch (Exception ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }
    }
}
