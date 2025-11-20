using DattingService.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Endpoints
{
    public static class ProfilesEndpoints
    {
        public static IEndpointRouteBuilder MapProfilesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/profiles/photo/upload", async (HttpContext context,
                [FromServices] IPhotoUploadHandler photoUploadHandler,
                [FromForm] IFormFile file,
                CancellationToken token) =>
            {
                return await photoUploadHandler.HandleAsync(context, file, token);
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
                [FromServices] IPhotoDeleteHandler photoDeleteHandler,
                CancellationToken token) =>
            {
                return await photoDeleteHandler.HandleAsync(context, token);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPut("/api/profiles/change", async (HttpContext context,
                [FromServices] IProfileChangeHandler profilesChangeHandler,
                [FromBody] RegistrRequest request,
                CancellationToken token) =>
            {
                return await profilesChangeHandler.HandleAsync(context, request, token);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPut("/api/profiles/password", async (HttpContext context,
                [FromServices] IProfilesPasswordHandler profilesPasswordHandler,
                [FromBody] string newPassword,
                CancellationToken token) =>
            {
                return await profilesPasswordHandler.Handle(context, newPassword, token);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPut("/api/profiles/interests/update", async (HttpContext context,
                [FromBody] InterestsRequest request,
                [FromServices] IInterestsUpdateHandler interestsUpdateHandler,
                CancellationToken token) =>
            {
                return await interestsUpdateHandler.HandleAsync(context, request, token);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/interests/{id}", async (Guid id, HttpContext context,
                [FromServices] IProfileInterestHandler profileInterestHandler,
                CancellationToken token) =>
            {
                return await profileInterestHandler.HandleAsync(context, id, token);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/interests/all", () =>
            {
                return Results.Ok(Interests.GetAll()); 
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/{id}", async (Guid id, HttpContext context, 
                [FromServices] IGetProfilesHandler getProfilesHandler,
                CancellationToken token) =>
            {
                return await getProfilesHandler.HandleAsync(context, id, token);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapGet("/api/profiles/photo/{photoName}", async (string photoName,
                HttpContext context,
                [FromServices] IProfilesPhotoHandler profilesHandler,
                CancellationToken token) =>
            {
                return await profilesHandler.HandleAsync(context, photoName, token);
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
