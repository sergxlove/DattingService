using Newtonsoft.Json.Linq;

namespace DattingService.Core.Models
{
    public class Interests
    {
        public Guid Id { get; private set; }

        public JArray SelectInterests { get; private set; } = new JArray();
        public enum Interest
        {

        }
    }
}
