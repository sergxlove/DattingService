namespace DataAccess.Photo.S3Minio.Abstractions
{
    public interface IPhotoRepository
    {
        Task CreateBucketIfNotExistsAsync(string bucketName, CancellationToken token);
        Task<bool> DeleteAsync(string bucketName, string fileName, CancellationToken token);
        Task<Stream?> DownloadFromNameAsync(string fileName, string bucketName, CancellationToken token);
        Task<bool> ExistsObjectAsync(string bucketName, string fileName, CancellationToken token);
        Task<string> UploadFileAsync(string bucketName, string fileName, Stream fileStream, CancellationToken token);
    }
}