using ProfilesServiceAPI.Abstractions;

namespace ProfilesServiceAPI.Services
{
    public class ConvertService : IConvertService
    {
        public async Task<byte[]> ConvertFormFileToByteArray(IFormFile file)
        {
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public IFormFile ConvertByteArrayToFormFile(byte[] fileBytes, string nameFile)
        {
            using var memoryStream = new MemoryStream();
            var stream = new MemoryStream(fileBytes);
            return new FormFile(stream, 0, fileBytes.Length, "file", nameFile); ;
        }
    }
}
