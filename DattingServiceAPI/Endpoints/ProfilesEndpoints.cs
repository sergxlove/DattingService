using DattingService.Core.Models;
using DattingService.Core.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Linq;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;
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
                [FromForm] IFormFile file,
                CancellationToken token) =>
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
                    string photoId = await photosService.UploadFileAsync("photopr", file.FileName,
                        stream, token);
                    if (photoId == string.Empty) return Results.InternalServerError();
                    Users? user = await usersService.GetByIdAsync(id, token);
                    if (user is null) return Results.NotFound("not found user");
                    user.AddUrlPhoto(photoId);
                    int resultUpdate = await usersService.UpdateAsync(user, token);
                    if (resultUpdate == 0) return Results.InternalServerError();
                    return Results.Ok();
                }
                catch(Exception ex)
                {
                    return Results.BadRequest(ex);
                }
            }).DisableAntiforgery()
            .RequireAuthorization("OnlyForAuthUser")
            .WithOpenApi(operation =>
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["file"] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                },
                                Required = new HashSet<string> { "file" }
                            }
                        }
                    }
                };
                return operation;
            });

            app.MapPost("/api/profiles/photo/delete", async (HttpContext context,
                [FromServices] IPhotosService photosService,
                [FromServices] IUsersService usersService,
                CancellationToken token) =>
            {
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                Users? user = await usersService.GetByIdAsync(id, token);
                if (user is null) return Results.NotFound("user is not found");
                if(user.PhotoURL is null) return Results.NotFound("this user no image");
                bool result = await photosService.DeleteAsync("photopr",user.PhotoURL[0].ToString(), token);
                if (result)
                {
                    user.RemoveUrlPhoto(user.PhotoURL[0].ToString());
                    await usersService.UpdateAsync(user, token);
                    return Results.Ok();
                }
                return Results.BadRequest("image don't delete");

            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPut("/api/profiles/change", async (HttpContext context,
                [FromServices] IUsersService usersService,
                [FromBody] RegistrRequest request,
                CancellationToken token) =>
            {
                if (request is null) return Results.BadRequest();
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                Users? user = await usersService.GetByIdAsync(id, token);
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

                Result<Users> newUser = Users.Create(usersRequest);
                if(newUser.IsSuccess)
                {
                    await usersService.UpdateAsync(newUser.Value, token);
                    return Results.Ok();
                }
                else
                {
                    return Results.BadRequest(newUser.Error);
                }

            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPut("/api/profiles/password", async (HttpContext context,
                [FromServices] ILoginUsersService loginService,
                [FromBody] string newPassword,
                CancellationToken token) =>
            {
                try
                {
                    string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    if (idStr == string.Empty) return Results.BadRequest("error");
                    Guid id = Guid.Parse(idStr!);
                    string email = await loginService.GetEmailAsync(id, token);
                    Result<LoginUsers> userLogin = LoginUsers.Create(id, email, newPassword);
                    if (!userLogin.IsSuccess) return Results.BadRequest(userLogin.Error);
                    await loginService.UpdatePasswordAsync(userLogin.Value, token);
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPut("/api/profiles/interests/update", async (HttpContext context,
                [FromBody] InterestsRequest request,
                [FromServices] IUsersService usersService,
                [FromServices] IInterestsService interestService,
                CancellationToken token) =>
            {
                string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                if (idStr == string.Empty) return Results.BadRequest("error");
                Guid id = Guid.Parse(idStr!);
                bool result = await interestService.CheckAsync(id, token);
                if (!result) return Results.NotFound("user not found");
                Interests interests = new(id, JArray.FromObject(request.InterestsUser));
                await interestService.UpdateAsync(interests, token);
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/interests/{id}", async (Guid id, HttpContext context,
                [FromServices] IInterestsService interestService,
                CancellationToken token) =>
            {
                JArray result = await interestService.GetAsync(id, token);
                return Results.Ok(result);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/interests/all", () =>
            {
                return Results.Ok(Interests.GetAll());
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/{id}", async (Guid id, HttpContext context, 
                [FromServices] IUsersService userService,
                CancellationToken token) =>
            {
                try
                {
                    Users? result = await userService.GetByIdAsync(id, token);
                    if(result is null) return Results.BadRequest("error");
                    return Results.Ok(result);
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/photo/{photoName}", async (string photoName,
                HttpContext context,
                [FromServices] IPhotosService photosService,
                CancellationToken token) =>
            {
                Stream? stream = await photosService.DownloadFromNameAsync(photoName, "photopr", token);
                if (stream is null) return Results.BadRequest();
                return Results.Stream(stream);
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
