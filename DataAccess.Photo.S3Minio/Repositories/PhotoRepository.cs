using DataAccess.Photo.S3Minio.Abstractions;
using Minio.DataModel.Args;
using System.IO;

namespace DataAccess.Photo.S3Minio.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly PhotoMinioContext _context;
        private readonly HttpClient _httpClient;

        public PhotoRepository(PhotoMinioContext context)
        {
            _context = context;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(30);
        }

        public async Task CreateBucketIfNotExistsAsync(string bucketName)
        {
            var existsArgs = new BucketExistsArgs().WithBucket(bucketName);
            bool isFound = await _context._minioClient.BucketExistsAsync(existsArgs);
            if (!isFound)
            {
                var makeArgs = new MakeBucketArgs().WithBucket(bucketName);
                await _context._minioClient.MakeBucketAsync(makeArgs);
            }
        }

        public async Task<string> UploadFileAsync(string bucketName, string fileName)
        {
            try
            {
                await CreateBucketIfNotExistsAsync(bucketName);
                string uniqueName = GenerateUniqueNameFile(fileName, "photo");
                string contentType = GetContentType(uniqueName);
                using Stream fileStream = File.OpenRead(fileName);
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(uniqueName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType);
                await _context._minioClient.PutObjectAsync(putObjectArgs);
                return uniqueName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<Stream?> DownloadFromNameAsync(string fileName, string bucketName)
        {
            var memoryStream = new MemoryStream();
            try
            {
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithCallbackStream(async stream =>
                    {
                        await stream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                    });
                await _context._minioClient.GetObjectAsync(getObjectArgs);
                return memoryStream;
            }
            catch
            {
                memoryStream.Dispose();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string bucketName, string fileName)
        {
            try
            {
                var args = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName);
                await _context._minioClient.RemoveObjectAsync(args);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExistsObjectAsync(string bucketName, string fileName)
        {
            try
            {
                var args = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName);
                await _context._minioClient.StatObjectAsync(args);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLowerInvariant();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }
        }

        private string GenerateUniqueNameFile(string fileName, string typeObjects)
        {
            string result = string.Empty;
            string extension = Path.GetExtension(fileName);
            string timestamp = DateTime.UtcNow.ToString("yyyyMMdd");
            string guidString = Guid.NewGuid().ToString();
            result = $"{typeObjects}-{timestamp}-{guidString}{extension}";
            return result;
        }
    }
}
