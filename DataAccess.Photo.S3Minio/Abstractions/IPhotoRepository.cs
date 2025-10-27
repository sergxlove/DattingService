namespace DataAccess.Photo.S3Minio.Abstractions
{
    public interface IPhotoRepository
    {
        Task CreateBucketIfNotExistsAsync(string bucketName);
        Task<bool> DeleteAsync(string bucketName, string fileName);
        Task<Stream?> DownloadFromNameAsync(string fileName, string bucketName);
        Task<bool> ExistsObjectAsync(string bucketName, string fileName);
        Task<string> UploadFileAsync(string bucketName, string fileName);
    }
}