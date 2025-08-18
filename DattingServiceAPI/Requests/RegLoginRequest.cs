namespace ProfilesServiceAPI.Requests
{
    public class RegLoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AgainPassword {  get; set; } = string.Empty;
    }
}
