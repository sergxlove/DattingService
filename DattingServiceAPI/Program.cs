using DataAccess.Photo.S3Minio.Abstractions;
using DataAccess.Photo.S3Minio.Repositories;
using DataAccess.Profiles.Postgres;
using DataAccess.Profiles.Postgres.Abstractions;
using DataAccess.Profiles.Postgres.Infrastructure;
using DataAccess.Profiles.Postgres.Repositories;
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
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Extensions;
using ProfilesServiceAPI.Handlers.LoginHandlers;
using ProfilesServiceAPI.Handlers.ProfilesHandlers;
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
                options.UseNpgsql(builder.Configuration["ConnectionStrings:Default"]));
            builder.Services.AddScoped<IUsersRepository, UsersRepository>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<ILoginUsersRepository, LoginUsersRepository>();
            builder.Services.AddScoped<ILoginUsersService, LoginUsersService>();
            builder.Services.AddScoped<IInterestsRepository, InterestsRepository>();
            builder.Services.AddScoped<IInterestsService, InterestsService>();
            builder.Services.AddScoped<ITempLoginUsersRepository, TempLoginUsersRepository>();
            builder.Services.AddScoped<ITempLoginUsersService, TempLoginUsersService>();
            builder.Services.AddScoped<ITokensUserRepository, TokensUserRepository>();
            builder.Services.AddScoped<ITokensUserService, TokensUserService>();
            builder.Services.AddScoped<ITransactionsWork, TransactionsWork>();
            builder.Services.AddScoped<IRegistrUserService, RegistrUserService>();
            builder.Services.AddScoped<IPhotoMovedService, PhotoMovedService>();
            builder.Services.AddScoped<IConvertService, ConvertService>();
            builder.Services.AddSingleton<IMinioClient>(sp =>
                new MinioClient()
                    .WithEndpoint(builder.Configuration["MinioS3:Address"])
                    .WithCredentials(builder.Configuration["MinioS3:User"], builder.Configuration["MinioS3:Password"])
                    .WithSSL(false)
                    .Build());
            builder.Services.AddScoped<IPhotoRepository, PhotoRepository>();
            builder.Services.AddScoped<IPhotosService, PhotosService>();
            builder.Services.AddScoped<IJwtProviderService, JwtProviderService>();
            builder.Services.AddScoped<IDeleteUserHandler, DeleteUserHandler>();
            builder.Services.AddScoped<ILoginUserHandler, LoginUserHandler>();
            builder.Services.AddScoped<ILogoutHandler, LogoutHandler>();
            builder.Services.AddScoped<IRefreshHandler, RefreshHandler>();
            builder.Services.AddScoped<IRegLoginUserHandler, RegLoginUserHandler>();
            builder.Services.AddScoped<IRegUserHandler, RegUserHandler>();
            builder.Services.AddScoped<IGetProfilesHandler, GetProfilesHandler>();
            builder.Services.AddScoped<IInterestsUpdateHandler, InterestsUpdateHandler>();
            builder.Services.AddScoped<IPhotoDeleteHandler, PhotoDeleteHandler>();
            builder.Services.AddScoped<IPhotoUploadHandler, PhotoUploadHandler>();
            builder.Services.AddScoped<IProfileChangeHandler,  ProfileChangeHandler>();
            builder.Services.AddScoped<IProfileInterestHandler, ProfileInterestHandler>();
            builder.Services.AddScoped<IProfilesPasswordHandler, ProfilesPasswordHandler>();
            builder.Services.AddScoped<IProfilesPhotoHandler, ProfilesPhotoHandler>();
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Limits.MaxRequestBodySize = 10 * 1024 * 1024; 
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    IConfigurationSection? jwtSettings = builder.Configuration
                        .GetSection("JwtSettings");
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidateAudience = true,
                        ValidAudience = jwtSettings["Audience"],
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                            .GetBytes(jwtSettings["SecretKey"]!)),
                        ValidateIssuerSigningKey = true
                    };
                    options.Events = new JwtBearerEvents()
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["access_token"];
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
                options.AddPolicy("OnlyForBeginRegUser", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, "beginRegUser");
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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocal", policy =>
                {
                    policy.WithOrigins("*") 
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });


            WebApplication app = builder.Build();
            app.UseCors("AllowLocal");
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
