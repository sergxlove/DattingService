using DataAccess.Profiles.Postgres;
using Microsoft.EntityFrameworkCore;

namespace ProfilesServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("D:\\projects\\DattingService\\appsetings.json");
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProfilesDbContext>(options =>
                options.UseNpgsql(builder.Configuration
                .GetSection("DataAccess.Profiles.Postgres:Connections:ConnectionStrings")
                .Value));
            var app = builder.Build();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseWelcomePage();

            app.Run();
        }
    }
}
