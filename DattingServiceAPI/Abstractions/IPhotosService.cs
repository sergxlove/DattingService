namespace ProfilesServiceAPI.Abstractions
{
    public interface IPhotosService
    {
        Task CreateBucketIfNotExistsAsync(string bucketName, CancellationToken token);
        Task<string> UploadFileAsync(string bucketName, string fileName, Stream fileStream, CancellationToken token);
        Task<Stream?> DownloadFromNameAsync(string fileName, string bucketName, CancellationToken token);
        Task<bool> DeleteAsync(string bucketName, string fileName, CancellationToken toekn);
        Task<bool> ExistsObjectAsync(string bucketName, string fileName, CancellationToken token);
    }
}