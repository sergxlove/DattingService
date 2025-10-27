using Minio;

namespace DataAccess.Photo.S3Minio
{
    public class PhotoMinioContext
    {
        private readonly string _endpoint = string.Empty;
        private readonly string _accessKey = string.Empty;
        private readonly string _secretKey = string.Empty;
        public readonly IMinioClient _minioClient;

        public PhotoMinioContext()
        {
            _endpoint = "45.134.12.203:9000";
            _accessKey = "TLjPyZnRNC0N3nsO";
            _secretKey = "E8cnUDQ6fDDQeAQY";
            _minioClient = new MinioClient()
                .WithEndpoint(_endpoint)
                .WithCredentials(_accessKey, _secretKey)
                .WithSSL(false)
                .Build();
        }

        public PhotoMinioContext(string endpoint = "localhost:9000", string accessKey = "minioadmin",
            string secretKey = "minioadmin")
        {
            _endpoint = endpoint;
            _accessKey = accessKey;
            _secretKey = secretKey;
            _minioClient = new MinioClient()
                .WithEndpoint(_endpoint)
                .WithCredentials(_accessKey, _secretKey)
                .WithSSL(false)
                .Build();
        }
    }
}
