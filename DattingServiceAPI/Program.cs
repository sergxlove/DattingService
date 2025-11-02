using DataAccess.Photo.S3Minio;
using DataAccess.Photo.S3Minio.Abstractions;
using DataAccess.Photo.S3Minio.Repositories;
using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Abstractions;
using DataAccess.Profiles.Postgres.Infrastructure;
using DattingService.Core.Abstractions;
using DattingService.Core.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Minio;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Extensions;
using ProfilesServiceAPI.Repositories;
using ProfilesServiceAPI.Services;
using Serilog;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace ProfilesServiceAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("D:\\projects\\DattingService\\appsetings.json");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .WriteTo.File("D:\\projects\\DattingService\\log.txt")
                .CreateLogger();
            //builder.Host.UseSerilog();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IPasswordHasherService, PasswordHasherService>();
            builder.Services.AddScoped<IPasswordValidatorService, PasswordValidatorService>();
            builder.Services.AddDbContext<ProfilesDbContext>(options =>
                options.UseNpgsql("User ID=postgres;Password=123;Host=localhost;Port=5432;Database=db;"));
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<ILoginUsersRepository, LoginUsersRepository>();
            builder.Services.AddScoped<ILoginUsersService, LoginUsersService>();
            builder.Services.AddScoped<IInterestsRepository, InterestsRepository>();
            builder.Services.AddScoped<IInterestsService, InterestsService>();
            builder.Services.AddScoped<ITempLoginUsersRepository, TempLoginUsersRepository>();
            builder.Services.AddScoped<ITempLoginUsersService, TempLoginUsersService>();
            builder.Services.AddScoped<ITransactionsWork, TransactionsWork>();
            builder.Services.AddScoped<IRegistrUserService, RegistrUserService>();
            builder.Services.AddScoped<IConvertService, ConvertService>();
            builder.Services.AddSingleton<IMinioClient>(sp =>
                new MinioClient()
                    .WithEndpoint("localhost:9000")
                    .WithCredentials("TLjPyZnRNC0N3nsO", "E8cnUDQ6fDDQeAQY")
                    .WithSSL(false)
                    .Build());
            builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
            builder.Services.AddScoped<IPhotosService, PhotosService>();
            builder.Services.AddScoped<IJwtProviderService, JwtProviderService>();
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; 
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

            builder.Services.AddRateLimiter(options =>
            {
                options.AddFixedWindowLimiter("GeneralPolicy", opt =>
                {
                    opt.PermitLimit = 100;
                    opt.Window = TimeSpan.FromMinutes(1);
                    opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    opt.QueueLimit = 10;
                });
                options.AddFixedWindowLimiter("LoginPolicy", opt =>
                {
                    opt.PermitLimit = 5;
                    opt.Window = TimeSpan.FromMinutes(1);
                });
                options.AddTokenBucketLimiter("UploadPolicy", opt =>
                {
                    opt.TokenLimit = 10;
                    opt.ReplenishmentPeriod = TimeSpan.FromMinutes(1);
                    opt.TokensPerPeriod = 2;
                    opt.AutoReplenishment = true;
                });
            });

            builder.Services.AddHealthChecks()
                .AddCheck("self", () =>
                {
                    return HealthCheckResult.Healthy("ok");
                });


            WebApplication app = builder.Build();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.MapAllEndpoints();

            app.Run();
        }
    }
}
