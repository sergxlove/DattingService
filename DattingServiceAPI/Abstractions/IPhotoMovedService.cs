namespace ProfilesServiceAPI.Abstractions
{
    public interface IPhotoMovedService
    {
        Task<bool> AddPhotoAsync(Stream photoStream, Guid idUser, string fileName, CancellationToken token);
        Task<bool> DeletePhotoAsync(Guid idUser, CancellationToken token);
    }
}