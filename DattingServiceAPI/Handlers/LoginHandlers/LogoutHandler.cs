using ProfilesServiceAPI.Abstractions.Handlers;

namespace ProfilesServiceAPI.Handlers.LoginHandlers
{
    public class LogoutHandler : ILogoutHandler
    {
        public LogoutHandler() { }

        public IResult HandleAsync(HttpContext context)
        {
            context.Response.Cookies.Delete("access_token");
            context.Response.Cookies.Delete("refresh_token");
            return Results.Ok();
        }
    }
}
