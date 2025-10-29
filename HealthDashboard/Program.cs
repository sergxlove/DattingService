namespace HealthDashboard
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
           
            builder.Services.AddHealthChecksUI(setup =>
            {
                setup.AddHealthCheckEndpoint("ProfilesServiceAPI", "http://localhost:7006/health");
            }).AddInMemoryStorage();
            var app = builder.Build();
            app.MapHealthChecksUI();
            app.Run();
        }
    }
}
