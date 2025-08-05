using DataAccess.Photo.MongoDB.Abstractions;
using DataAccess.Photo.MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Photo.MongoDB.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoDbContext _context;
        public PhotoRepository(PhotoDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAsync(Photos photo)
        {
            await _context.PhotosCollection.InsertOneAsync(photo);
            return photo.Id.ToString();
        }

        public async Task<Photos?> ReadAsync(string id)
        {
            ObjectId photoId = ObjectId.Parse(id);
            var photo = await _context.PhotosCollection
                .Find(p => p.Id == photoId)
                .FirstOrDefaultAsync();
            return photo;
        }

        public async Task<long> DeleteAsync(string id)
        {
            ObjectId photoId = ObjectId.Parse(id);
            var result = await _context.PhotosCollection.DeleteOneAsync(a => a.Id.ToString() == id);
            return result.DeletedCount;
        }


    }
}
