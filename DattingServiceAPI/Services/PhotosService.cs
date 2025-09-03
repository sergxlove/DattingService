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
        public async Task<string> AddAsync(Stream stream, string fileName, string contentType, Guid userId, CancellationToken token)
        {
            return await _repository.AddAsync(stream, fileName, contentType, userId, token);
        }
        public async Task<bool> DeleteAsync(string id, CancellationToken token)
        {
            return await _repository.DeleteAsync(id, token);
        }
        public async Task<Stream> ReadAsync(string id, CancellationToken token)
        {
            return await _repository.ReadAsync(id, token);
        }
    }
}
