using Newtonsoft.Json.Linq;

namespace DataAccess.Profiles.Postgres.Models
{
    public class InterestsEntity
    {
        public Guid Id { get; set; }

        public int[] SelectInterests { get; set; } = Array.Empty<int>();
    }
}
