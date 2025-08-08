using DataAccess.Photo.MongoDB.Models;
using DataAccess.Profiles.Postgres.Models;
using DattingService.Core.Models;
using Microsoft.AspNetCore.Mvc;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Requests;
using System.Security.Claims;

namespace ProfilesServiceAPI.Endpoints
{
    public static class LogInEndpoints
    {
        public static IEndpointRouteBuilder MapLogInEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("/api/users/logout", (HttpContext context) =>
            {
                context.Response.Cookies.Delete("jwt");
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            app.MapPost("/api/users/login", async (HttpContext context,
                [FromBody] LoginRequest request,
                [FromServices] IUsersLoginService usersService,
                [FromServices] IJwtProviderService jwtGenerate) =>
            {
                try
                {
                    if (request.Username == string.Empty || request.Password == string.Empty)
                        return Results.BadRequest("login or password is empty");
                    
                    if (await usersService!.CheckAsync(request.Username))
                        return Results.BadRequest("user is not found");
                    
                    if (!await usersService!.VerifyAsync(request.Username, request.Password))
                        return Results.BadRequest("no auth");
                    
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Role, "user")
                    };
                    var token = jwtGenerate!.GenerateToken(new JwtRequest()
                    {
                        Claims = claims
                    });
                    context.Response.Cookies.Append("jwt", token!);
                    return Results.Ok(token!);
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapPost("/api/users/reg", async (HttpContext context, 
                IFormFile file, 
                RegistrRequest request) =>
            {
                try
                {
                    if (request is null) return Results.BadRequest("data is null");
                    if (file == null || file.Length == 0) return Results.BadRequest("No file uploaded");
                    if (!file.FileName.EndsWith(".png") && !file.FileName.EndsWith(".jpg"))
                        return Results.BadRequest("file is not .png, .jpg");
                    var userLoginService = app.ServiceProvider.GetService<IUsersLoginService>();
                    var usersService = app.ServiceProvider.GetService<IUsersService>();
                    var photoService = app.ServiceProvider.GetService<IPhotosService>();
                    var convertService = app.ServiceProvider.GetService<IConvertService>();
                    Guid userId = Guid.NewGuid();
                    Photos photo = new Photos()
                    {
                        UserId = userId,
                        Image = await convertService!.ConvertFormFileToByteArray(file),
                        ContentType = "image"
                    };
                    string urlPhoto = await photoService!.CreateAsync(photo);
                    var userData = UsersDataForLogin.Create(request.Username, request.Password);
                    if (userData.error != string.Empty) return Results.BadRequest(userData.error);
                    var user = Users.Create(userId, request.Name, request.Age, request.Description,
                        request.City, urlPhoto, true);
                    if (user.error != string.Empty) return Results.BadRequest(user.error);
                    await userLoginService!.AddAsync(userData.user!);
                    await usersService!.AddAsync(user.user!);
                    return Results.Ok();
                }
                catch
                {
                    return Results.BadRequest("error");
                }
            });

            app.MapDelete("/api/users/delete", (HttpContext context) =>
            {
                return Results.Ok();
            }).RequireAuthorization("OnlyForAuthUser");

            return app;
        }
    }
}
