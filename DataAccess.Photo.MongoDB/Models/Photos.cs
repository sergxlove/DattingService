using MongoDB.Bson;

namespace DataAccess.Photo.MongoDB.Models
{
    public class Photos
    {
        public ObjectId Id { get; set; } = ObjectId.GenerateNewId();
        public Guid UserId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public ObjectId GridFsFileId { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    }
}
