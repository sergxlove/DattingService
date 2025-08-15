namespace ProfilesServiceAPI.Requests
{
    public class RegistrRequest
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Target { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }
}
