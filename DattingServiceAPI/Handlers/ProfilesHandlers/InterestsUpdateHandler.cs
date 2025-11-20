using DattingService.Core.Models;
using Microsoft.AspNetCore.Mvc;
using ProfilesServiceAPI.Abstractions;
using ProfilesServiceAPI.Abstractions.Handlers;
using ProfilesServiceAPI.Requests;
using ProfilesServiceAPI.Services;
using System.Security.Claims;

namespace ProfilesServiceAPI.Handlers.ProfilesHandlers
{
    public class InterestsUpdateHandler : IInterestsUpdateHandler
    {
        private readonly IUsersService _usersService;
        private readonly IInterestsService _interestService;

        public InterestsUpdateHandler(IUsersService usersService, IInterestsService interestsService)
        {
            _interestService = interestsService;
            _usersService = usersService;
        }

        public async Task<IResult> HandleAsync(HttpContext context, InterestsRequest request,
            CancellationToken token)
        {
            string? idStr = context.User.FindFirst(ClaimTypes.Sid)?.Value;
            if (idStr == string.Empty) return Results.BadRequest("error");
            Guid id = Guid.Parse(idStr!);
            bool result = await _interestService.CheckAsync(id, token);
            if (!result) return Results.NotFound("user not found");
            Interests interests = new(id, request.InterestsUser.ToArray());
            await _interestService.UpdateAsync(interests, token);
            return Results.Ok();
        }
    }
}
