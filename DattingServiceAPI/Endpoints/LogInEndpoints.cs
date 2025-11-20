using Microsoft.AspNetCore.Mvc;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Endpoints
{
    public static class LogInEndpoints
    {
        public static IEndpointRouteBuilder MapLogInEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/logout", (HttpContext context, 
                [FromServices] ILogoutHandler logoutHandler) =>
            {
                return logoutHandler.HandleAsync(context);
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/api/users/login", async (HttpContext context,
                [FromBody] LoginRequest request,
                [FromServices] ILoginUserHandler loginHandler,
                CancellationToken token) =>
            {
                return await loginHandler.HandleAsync(context, request, token);
            });

            app.MapPost("/api/refresh", async (HttpContext context,
                [FromServices] IRefreshHandler refreshHandler,
                CancellationToken token) =>
            {
                return await refreshHandler.HandleAsync(context, token);
            });

            app.MapPost("/api/users/regLoginUser", async (HttpContext context,
                [FromBody] RegLoginRequest request,
                [FromServices] IRegLoginUserHandler regLoginUserHandler,
                CancellationToken token) =>
            {
                return await regLoginUserHandler.HandleAsync(context, request, token);
            });

            app.MapPost("/api/users/regUser", async (HttpContext context,
                [FromBody] RegistrRequest request,
                [FromServices] IRegUserHandler regUserHandler,
                CancellationToken token) =>
            {
                return await regUserHandler.HandleAsync(context, request, token);
            }).RequireAuthorization("OnlyForBeginRegUser");

            app.MapDelete("/api/users/delete", async (HttpContext context,
                [FromServices] IDeleteUserHandler deleteHandler,
                [FromServices] IRegistrUserService registrService,
                CancellationToken token) =>
            {
                return await deleteHandler.HandleAsync(context, token);
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
