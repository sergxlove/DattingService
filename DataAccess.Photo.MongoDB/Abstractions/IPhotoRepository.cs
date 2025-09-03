using DataAccess.Photo.MongoDB.Models;

namespace DataAccess.Photo.MongoDB.Abstractions
{
    public interface IPhotoRepository
    {
        Task<string> AddAsync(Stream stream, string fileName, string contentType, Guid userId, CancellationToken token);
        Task<bool> DeleteAsync(string id, CancellationToken token);
        Task<Stream> ReadAsync(string id, CancellationToken token);
    }
}