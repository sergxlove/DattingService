namespace ProfilesServiceAPI.Abstractions.Handlers
{
    public interface IProfilesPasswordHandler
    {
        Task<IResult> Handle(HttpContext context, string newPassword, CancellationToken token);
    }
}