using System.Security.Claims;

namespace ProfilesServiceAPI.Requests
{
    public class JwtRequest
    {
        public string Issuer { get; set; } = "MyAuthServer";
        public string Audience { get; set; } = "MyAuthClients";
        public List<Claim> Claims { get; set; } = [];
        public DateTime Expires { get; set; } = DateTime.UtcNow.AddHours(1);
        public string SecretKey { get; set; } = "mysecretkeymysecretkeymysecretkey";
    }
}
