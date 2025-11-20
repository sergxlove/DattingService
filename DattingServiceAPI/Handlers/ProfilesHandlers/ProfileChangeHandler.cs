using DattingService.Core.Models;
using DattingService.Core.Requests;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class ProfileChangeHandler : IProfileChangeHandler
    {
        private readonly IUsersService _usersService;

        public ProfileChangeHandler(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, RegistrRequest request,
            CancellationToken token)
        {
            if (request is null) return Results.BadRequest();
            string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
            if (idStr == string.Empty) return Results.BadRequest("error");
            Guid id = Guid.Parse(idStr!);
            Users? user = await _usersService.GetByIdAsync(id, token);
            if (user is null) return Results.NotFound("user is not found");
            UsersRequest usersRequest = new()
            {
                Id = user.Id,
                PhotoURL = user.PhotoURL,
                IsActive = user.IsActive,
                IsVerify = user.IsVerify,
            };
            if (request.Name == string.Empty) usersRequest.Name = user.Name;
            else usersRequest.Name = request.Name;
            if (request.Age == 0) usersRequest.Age = user.Age;
            else usersRequest.Age = request.Age;
            if (request.Target == string.Empty) usersRequest.Target = user.Target;
            else usersRequest.Target = request.Target;
            if (request.City == string.Empty) usersRequest.City = user.City;
            else usersRequest.City = request.City;
            if (request.Description == string.Empty) usersRequest.Description = user.Description;
            else usersRequest.Description = request.Description;

            Result<Users> newUser = Users.Create(usersRequest);
            if (newUser.IsSuccess)
            {
                await _usersService.UpdateAsync(newUser.Value, token);
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest(newUser.Error);
            }
        }
    }
}
