using Newtonsoft.Json.Linq;

namespace DattingService.Core.Requests
{
    public class UsersRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Target { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string[] PhotoURL { get; set; } = Array.Empty<string>();
        public bool IsActive { get; set; }
        public bool IsVerify { get; set; }
    }
}
