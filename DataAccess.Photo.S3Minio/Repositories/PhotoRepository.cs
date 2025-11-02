using DataAccess.Photo.S3Minio.Abstractions;
using Minio;
using Minio.DataModel.Args;
using System.IO;

namespace DataAccess.Photo.S3Minio.Repositories
{
    public class PhotoRepository : IPhotoRepository
    {
        private readonly IMinioClient _context;
        private readonly HttpClient _httpClient;

        public PhotoRepository(IMinioClient context)
        {
            _context = context;
            _httpClient = new HttpClient();
            _httpClient.Timeout = TimeSpan.FromMinutes(30);
        }

        public async Task CreateBucketIfNotExistsAsync(string bucketName, CancellationToken token)
        {
            try
            {
                BucketExistsArgs existsArgs = new BucketExistsArgs().WithBucket(bucketName);
                bool isFound = await _context.BucketExistsAsync(existsArgs, token);
                if (!isFound)
                {
                    MakeBucketArgs makeArgs = new MakeBucketArgs().WithBucket(bucketName);
                    await _context.MakeBucketAsync(makeArgs, token);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public async Task<string> UploadFileAsync(string bucketName, string fileName, 
            Stream fileStream, CancellationToken token)
        {
            try
            {
                await CreateBucketIfNotExistsAsync(bucketName, token);
                string uniqueName = GenerateUniqueNameFile(fileName, "photo");
                string contentType = GetContentType(uniqueName);
                if(!fileStream.CanSeek)
                {
                    MemoryStream memoryStream = new();
                    await fileStream.CopyToAsync(memoryStream, token);
                    memoryStream.Position = 0;
                    fileStream = memoryStream;
                }
                PutObjectArgs putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(uniqueName)
                    .WithStreamData(fileStream)
                    .WithObjectSize(fileStream.Length)
                    .WithContentType(contentType);
                await _context.PutObjectAsync(putObjectArgs, token);
                return uniqueName;
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<Stream?> DownloadFromNameAsync(string fileName, string bucketName, 
            CancellationToken token)
        {
            MemoryStream memoryStream = new();
            try
            {
                GetObjectArgs getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName)
                    .WithCallbackStream(async stream =>
                    {
                        await stream.CopyToAsync(memoryStream);
                        memoryStream.Position = 0;
                    });
                await _context.GetObjectAsync(getObjectArgs, token);
                return memoryStream;
            }
            catch
            {
                memoryStream.Dispose();
                return null;
            }
        }

        public async Task<bool> DeleteAsync(string bucketName, string fileName,
            CancellationToken token)
        {
            try
            {
                RemoveObjectArgs args = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName);
                await _context.RemoveObjectAsync(args, token);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ExistsObjectAsync(string bucketName, string fileName, 
            CancellationToken token)
        {
            try
            {
                StatObjectArgs args = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(fileName);
                await _context.StatObjectAsync(args, token);
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
