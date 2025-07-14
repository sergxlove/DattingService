using DataAccess.Profiles.Postgres;
using Microsoft.EntityFrameworkCore;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Repositories;
using ProfilesServiceAPI.Services;
using Serilog;
using System.Security.Cryptography.X509Certificates;

namespace ProfilesServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("D:\\projects\\DattingService\\appsetings.json");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.File("")
                .CreateLogger();
            builder.Host.UseSerilog();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProfilesDbContext>(options =>
                options.UseNpgsql(builder.Configuration
                .GetSection("DataAccess.Profiles.Postgres:Connections:ConnectionStrings")
                .Value));
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
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
