namespace ProfilesServiceAPI.Handlers.LoginHandlers
{
    public class LogoutHandler
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
