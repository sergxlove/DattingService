using Microsoft.AspNetCore.Mvc;
using ProfilesServiceAPI.Abstractions;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.LoginHandlers
{
    public class DeleteHandler
    {

        private readonly IRegistrUserService _registrService;

        public DeleteHandler(IRegistrUserService registrUserService)
        {
            _registrService = registrUserService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, CancellationToken token)
        {
            string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
            if (idStr == string.Empty) return Results.BadRequest("error");
            Guid id = Guid.Parse(idStr!);
            bool result = await _registrService.DeleteUserAsync(id, token);
            if (!result) return Results.BadRequest("no delete");
            context.Response.Cookies.Delete("access_token");
            context.Response.Cookies.Delete("refresh_token");
            return Results.Ok();
        }
    }
}
