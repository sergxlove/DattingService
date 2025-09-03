namespace ProfilesServiceAPI.Abstractions
{
    public interface IPhotosService
    {
        Task<string> AddAsync(Stream stream, string fileName, string contentType, Guid userId, CancellationToken token);
        Task<bool> DeleteAsync(string id, CancellationToken token);
        Task<Stream> ReadAsync(string id, CancellationToken token);
    }
}