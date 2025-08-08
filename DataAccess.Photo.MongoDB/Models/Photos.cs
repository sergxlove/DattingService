using MongoDB.Bson;

namespace DataAccess.Photo.MongoDB.Models
{
    public class Photos
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();

        public Guid UserId { get; set; } 

        public required byte[] Image { get; set; }

        public string ContentType { get; set; } = string.Empty;
    }
}
