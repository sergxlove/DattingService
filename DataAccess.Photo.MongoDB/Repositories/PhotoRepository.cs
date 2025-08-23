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


        public async Task<Stream> ReadAsync(string id)
        {
            var memoryStream = new MemoryStream();
            await _context._gridFSBucket.DownloadToStreamAsync(ObjectId.Parse(id), memoryStream);
            memoryStream.Position = 0;
            return memoryStream;
        }

        public async Task<string> AddAsync(Stream stream, string fileName,
            string contentType, Guid userId)
        {
            using var session = await _context._client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                Photos photo = new()
                {
                    UserId = userId,
                    FileName = fileName,
                    ContentType = contentType,
                    GridFsFileId = await _context._gridFSBucket.UploadFromStreamAsync(fileName, stream),
                    UploadDate = DateTime.UtcNow,
                };
                await _context.PhotosCollection.InsertOneAsync(session, photo);
                await session.CommitTransactionAsync();
                return photo.Id.ToString();
            }
            catch
            {
                await session.AbortTransactionAsync();
                return string.Empty;
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            using var session = await _context._client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var photo = await _context.PhotosCollection
                    .Find(session, p => p.Id == ObjectId.Parse(id))
                    .FirstOrDefaultAsync();
                if (photo is null) return false;
                await _context._gridFSBucket.DeleteAsync(photo.GridFsFileId);
                await _context.PhotosCollection.DeleteOneAsync(session, a => a.Id == ObjectId.Parse(id));
                await session.CommitTransactionAsync();
                return true;
            }
            catch
            {
                await session.AbortTransactionAsync();
                return false;
            }
        }
    }
}
