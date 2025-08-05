using DataAccess.Photo.MongoDB.Abstractions;
using DataAccess.Photo.MongoDB.Models;
using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class PhotosService : IPhotosService
    {
        private readonly IPhotoRepository _repository;
        public PhotosService(IPhotoRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> CreateAsync(Photos photo)
        {
            return await _repository.CreateAsync(photo);
        }
        public async Task<long> DeleteAsync(string id)
        {
            return await _repository.DeleteAsync(id);
        }
        public async Task<Photos?> ReadAsync(string id)
        {
            return await _repository.ReadAsync(id);
        }
    }
}
