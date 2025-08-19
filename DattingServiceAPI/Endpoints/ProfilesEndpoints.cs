using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ProfilesServiceAPI.Services;
using System.Security.Claims;

namespace ProfilesServiceAPI.Endpoints
{
    public static class ProfilesEndpoints
    {
        public static IEndpointRouteBuilder MapProfilesEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapPost("/api/profiles/photo/upload", (HttpContext context,
                [FromBody] IFormFile file) =>
            {
                if (file is null || file.Length == 0)
                    return Results.BadRequest("No file uploaded");
                if (file.Length > 10 * 1024 * 1024)
                    return Results.BadRequest("File too large (max 10MB)");

                return Results.Ok();
            }).RequireAuthorization("user");

            app.MapPost("/api/profiles/photo/delete", () =>
            {

            }).RequireAuthorization("user");

            app.MapPost("/api/profiles/change", async (HttpContext context,
                [FromServices] UsersService usersService,
                [FromBody] RegisterRequest request) =>
            {
                await Task.CompletedTask;
            }).RequireAuthorization("user");

            app.MapPost("/api/profiles/password", async (HttpContext context,
                [FromServices] LoginUsersService loginService,
                string newPassword) =>
            {
                try
                {
                    var idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
                    if (idStr == string.Empty) return Results.BadRequest("error");
                    Guid id = Guid.Parse(idStr!);

                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            }).RequireAuthorization("user");

            app.MapPost("/api/profiles/interests/add", () =>
            {

            }).RequireAuthorization("user");

            app.MapDelete("/api/profiles/interests/delete", () =>
            {

            }).RequireAuthorization("user");

            app.MapGet("/api/profiles", () =>
            {

            }).RequireAuthorization("user");

            app.MapGet("/api/profiles/photo", () =>
            {

            }).RequireAuthorization("user");

            return app;
        }
    }
}
