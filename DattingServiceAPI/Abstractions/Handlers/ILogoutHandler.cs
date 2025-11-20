namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface ILogoutHandler
    {
        IResult HandleAsync(HttpContext context);
    }
}