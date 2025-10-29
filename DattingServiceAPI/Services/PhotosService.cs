using DataAccess.Photo.S3Minio.Abstractions;
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
        
        public async Task CreateBucketIfNotExistsAsync(string bucketName, CancellationToken token)
        {
            await _repository.CreateBucketIfNotExistsAsync(bucketName, token);
        }

        public async Task<string> UploadFileAsync(string bucketName, string fileName,
            Stream fileStream, CancellationToken token)
        {
            return await _repository.UploadFileAsync(bucketName, fileName, fileStream ,token);
        }

        public async Task<Stream?> DownloadFromNameAsync(string fileName, string bucketName,
            CancellationToken token)
        {
            return await _repository.DownloadFromNameAsync(fileName, bucketName, token);
        }

        public async Task<bool> DeleteAsync(string bucketName, string fileName, 
            CancellationToken token)
        {
            return await _repository.DeleteAsync(bucketName, fileName, token);
        }

        public async Task<bool> ExistsObjectAsync(string bucketName, string fileName, 
            CancellationToken token)
        {
            return await _repository.ExistsObjectAsync(bucketName, fileName, token);
        }
    }
}
