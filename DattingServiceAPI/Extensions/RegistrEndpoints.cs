using ProfilesServiceAPI.Endpoints;

namespace ProfilesServiceAPI.Extensions
{
    public static class RegistrEndpoints
    {
        public static IEndpointRouteBuilder MapAllEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapLogInEndpoints();
            app.MapProfilesEndpoints();
            return app;
        }
    }
}
