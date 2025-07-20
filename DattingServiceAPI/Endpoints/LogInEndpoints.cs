using ProfilesServiceAPI.Requests;

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

            app.MapPost("/api/users/login", (HttpContext context) =>
            {
                return Results.Ok();
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
