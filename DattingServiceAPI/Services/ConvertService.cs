using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class ConvertService : IConvertService
    {
        public async Task<byte[]> ConvertFormFileToByteArray(IFormFile file)
        {
            using MemoryStream memoryStream = new();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public IFormFile ConvertByteArrayToFormFile(byte[] fileBytes, string nameFile)
        {
            using MemoryStream memoryStream = new();
            MemoryStream stream = new(fileBytes);
            return new FormFile(stream, 0, fileBytes.Length, "file", nameFile); ;
        }
    }
}
