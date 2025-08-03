using DattingService.Core.Models;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Endpoints
{
    public static class LogInEndpoints
    {
        public static IEndpointRouteBuilder MapLogInEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/logout", (HttpContext context) =>
            {
                context.Response.Cookies.Delete("jwt");
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/api/users/login", async (HttpContext context, LoginRequest request) =>
            {
                if (request.Username == string.Empty || request.Password == string.Empty)
                {
                    return Results.BadRequest("login or password is empty");
                }
                var usersService = app.ServiceProvider.GetService<IUsersLoginService>();
                if (await usersService!.CheckAsync(request.Username))
                {
                    return Results.BadRequest("user is not found");
                }
                if (!await usersService!.VerifyAsync(request.Username, request.Password))
                {
                    return Results.BadRequest("no auth");
                }
                var jwtGenerate = context.RequestServices.GetService<IJwtProviderService>();
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Role, "user")
                };
                var token = jwtGenerate!.GenerateToken(new JwtRequest()
                {
                    Claims = claims
                });
                context.Response.Cookies.Append("jwt", token!);
                return Results.Ok(token!);
            });

            app.MapPost("/api/users/reg", (HttpContext context) =>
            {
                return Results.Ok();
            });

            app.MapDelete("/api/users/delete", (HttpContext context) =>
            {
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
