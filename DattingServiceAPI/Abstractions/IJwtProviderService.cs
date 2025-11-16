using ProfilesServiceAPI.Requests;

namespace ProfilesServiceAPI.Abstractions
{
    public interface IJwtProviderService
    {
        string GenerateToken(JwtRequest request);
    }
}