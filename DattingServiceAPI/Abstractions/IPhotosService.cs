using DataAccess.Photo.MongoDB.Models;

namespace ProfilesServiceAPI.Abstractions
{
    public interface IPhotosService
    {
        Task<string> CreateAsync(Photos photo);
        Task<long> DeleteAsync(string id);
        Task<Photos?> ReadAsync(string id);
    }
}