namespace ProfilesServiceAPI.Abstractions
{
    public interface IConvertService
    {
        IFormFile ConvertByteArrayToFormFile(byte[] fileBytes, string nameFile);
        Task<byte[]> ConvertFormFileToByteArray(IFormFile file);
    }
}