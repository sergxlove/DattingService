using DataAccess.Photo.MongoDB.Models;

namespace DataAccess.Photo.MongoDB.Abstractions
{
    public interface IPhotoRepository
    {
        Task<string> AddAsync(Stream stream, string fileName, string contentType, Guid userId);
        Task<bool> DeleteAsync(string id);
        Task<Stream> ReadAsync(string id);
    }
}