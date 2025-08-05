using DataAccess.Photo.MongoDB;
using DataAccess.Profiles.Postgres;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Extensions;
using ProfilesServiceAPI.Repositories;
using ProfilesServiceAPI.Services;
using Serilog;
using System.Security.Claims;
using System.Text;

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
                .WriteTo.Console()
                .WriteTo.File("D:\\projects\\DattingService\\log.txt")
                .CreateLogger();
            //builder.Host.UseSerilog();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ProfilesDbContext>(options =>
                options.UseNpgsql(builder.Configuration
                .GetSection("DataAccess.Profiles.Postgres:Connections:ConnectionStrings")
                .Value));
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IUsersLoginRepository, UsersLoginRepository>();
            builder.Services.AddScoped<IUsersLoginService, UsersLoginService>();
            builder.Services.AddSingleton<PhotoDbContext>(options =>
            {
                return new PhotoDbContext(
                    connectionString: "mongodb://localhost:27017",
                    nameDatabase: "photo"
                    );
            });
           

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    IConfigurationSection? jwtSettings = builder.Configuration
                        .GetSection("ProfilesServiceAPI:JwtSettings");
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = false,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidateAudience = false,
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = false,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(jwtSettings["SecretKey"]!)),
                        ValidateIssuerSigningKey = true
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["jwt"];
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyForAdmin", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "admin");
                });
                options.AddPolicy("OnlyForAuthUser", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "user");
                });
            });

            var app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.MapAllEndpoints();

            app.Run();
        }
    }
}
