using DataAccess.Photo.MongoDB.Models;

namespace DataAccess.Photo.MongoDB.Abstractions
{
    public interface IPhotoRepository
    {
        Task<string> CreateAsync(Photos photo);
        Task<long> DeleteAsync(string id);
        Task<Photos?> ReadAsync(string id);
    }
}