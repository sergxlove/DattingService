namespace HealthDashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddHealthChecksUI(setup =>
            {
                setup.AddHealthCheckEndpoint("ProfilesServiceAPI", "http://localhost:5295/health");
            }).AddInMemoryStorage();
            WebApplication app = builder.Build();
            app.MapHealthChecksUI();
            app.Run();
        }
    }
}
