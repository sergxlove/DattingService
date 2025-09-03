using DattingService.Core.Models;
using DattingService.Core.Requests;
using Microsoft.AspNetCore.Mvc;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;
using ProfilesServiceAPI.Services;
using System.Security.Claims;

namespace ProfilesServiceAPI.Endpoints
{
    public static class ProfilesEndpoints
    {
        public static IEndpointRouteBuilder MapProfilesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/profiles/photo/upload", async (HttpContext context,
                [FromServices] IPhotosService photosService,
                [FromServices] IUsersService usersService,
                [FromBody] IFormFile file,
                CancellationToken token) =>
            {
                try
                {
                    if (file is null || file.Length == 0)
                        return Results.BadRequest("No file uploaded");
                    if (file.Length > 10 * 1024 * 1024)
                        return Results.BadRequest("File too large (max 10MB)");
                    var idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    if (idStr == string.Empty) return Results.BadRequest("error");
                    Guid id = Guid.Parse(idStr!);
                    using var stream = file.OpenReadStream();
                    var photoId = await photosService.AddAsync(stream, file.FileName,
                        file.ContentType, id, token);
                    if (photoId == string.Empty) return Results.InternalServerError();
                    var user = await usersService.GetByIdAsync(id, token);
                    if (user is null) return Results.NotFound("not found user");
                    user.AddUrlPhoto(photoId);
                    int resultUpdate = await usersService.UpdateAsync(user, token);
                    if (resultUpdate == 0) return Results.InternalServerError();
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            }).RequireAuthorization("user");

            app.MapPost("/api/profiles/photo/delete", async (HttpContext context,
                [FromServices] IPhotosService photosService,
                [FromServices] IUsersService usersService,
                CancellationToken token) =>
            {
                var idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                var user = await usersService.GetByIdAsync(id, token);
                if (user is null) return Results.NotFound("user is not found");
                if(user.PhotoURL is null) return Results.NotFound("this user no image");
                var result = await photosService.DeleteAsync(user.PhotoURL[0].ToString(), token);
                if (result)
                {
                    user.RemoveUrlPhoto(user.PhotoURL[0].ToString());
                    await usersService.UpdateAsync(user, token);
                    return Results.Ok();
                }
                return Results.BadRequest("image dont delete");

            }).RequireAuthorization("user");

            app.MapPut("/api/profiles/change", async (HttpContext context,
                [FromServices] IUsersService usersService,
                [FromBody] RegistrRequest request,
                CancellationToken token) =>
            {
                if (request is null) return Results.BadRequest();
                var idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                var user = await usersService.GetByIdAsync(id, token);
                if(user is null) return Results.NotFound("user is not found");
                UsersRequest usersRequest = new()
                {
                    Id = user.Id,
                    PhotoURL = user.PhotoURL,
                    IsActive = user.IsActive,
                    IsVerify = user.IsVerify,
                };
                if(request.Name == string.Empty) usersRequest.Name = user.Name;
                else usersRequest.Name = request.Name;
                if(request.Age == 0) usersRequest.Age = user.Age;
                else usersRequest.Age = request.Age;
                if(request.Target == string.Empty) usersRequest.Target = user.Target;
                else usersRequest.Target = request.Target;
                if(request.City == string.Empty) usersRequest.City = user.City;
                else usersRequest.City = request.City;
                if(request.Description == string.Empty) usersRequest.Description = user.Description;
                else usersRequest.Description = request.Description;

                var newUser = Users.Create(usersRequest);
                if(newUser.IsSuccess)
                {
                    await usersService.UpdateAsync(newUser.Value, token);
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest(newUser.Error);
                }

            }).RequireAuthorization("user");

            app.MapPut("/api/profiles/password", async (HttpContext context,
                [FromServices] LoginUsersService loginService,
                [FromBody] string newPassword,
                CancellationToken token) =>
            {
                try
                {
                    var idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    if (idStr == string.Empty) return Results.BadRequest("error");
                    Guid id = Guid.Parse(idStr!);
                    string email = await loginService.GetEmailAsync(id, token);
                    var userLogin = LoginUsers.Create(id, email, newPassword);
                    if (!userLogin.IsSuccess) return Results.BadRequest(userLogin.Error);
                    await loginService.UpdatePasswordAsync(userLogin.Value, token);
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            }).RequireAuthorization("user");

            app.MapPut("/api/profiles/interests/add", () =>
            {

            }).RequireAuthorization("user");

            app.MapDelete("/api/profiles/interests/delete", () =>
            {

            }).RequireAuthorization("user");

            app.MapGet("/api/profiles/interests", () =>
            {

            }).RequireAuthorization("user");

            app.MapGet("/api/profiles/interests/all", () =>
            {

            }).RequireAuthorization("user");

            app.MapGet("/api/profiles", async (HttpContext context, 
                [FromServices] IUsersService userService,
                [FromBody] Guid id,
                CancellationToken token) =>
            {
                try
                {
                    var result = await userService.GetByIdAsync(id, token);
                    if(result is null) return Results.BadRequest("error");
                    return Results.Ok(result);
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            }).RequireAuthorization("user");

            app.MapGet("/api/profiles/photo", async (HttpContext context,
                [FromServices] IPhotosService photosService,
                [FromBody] string id,
                CancellationToken token) =>
            {
                var imageStream = await photosService.ReadAsync(id, token);
                return Results.File(imageStream);
            }).RequireAuthorization("user");

            return app;
        }
    }
}
