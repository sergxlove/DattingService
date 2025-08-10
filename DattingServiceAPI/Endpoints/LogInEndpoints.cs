using DataAccess.Photo.MongoDB.Models;
using DattingService.Core.Models;
using Microsoft.AspNetCore.Mvc;
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

            app.MapPost("/api/users/login", async (HttpContext context,
                [FromBody] LoginRequest request,
                [FromServices] ILoginUsersService loginUserService,
                [FromServices] IJwtProviderService jwtGenerate) =>
            {
                try
                {
                    if (request.Username == string.Empty || request.Password == string.Empty)
                        return Results.BadRequest("login or password is empty");
                    
                    if (await loginUserService!.CheckAsync(request.Username))
                        return Results.BadRequest("user is not found");
                    
                    if (!await loginUserService!.VerifyAsync(request.Username, request.Password))
                        return Results.BadRequest("no auth");
                    
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
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapPost("/api/users/reg", async (HttpContext context, 
                [FromBody] RegistrRequest request) =>
            {
                try
                {
                    await Task.CompletedTask;
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapDelete("/api/users/delete", (HttpContext context) =>
            {
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
