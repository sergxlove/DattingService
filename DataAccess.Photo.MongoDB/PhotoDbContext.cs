using DataAccess.Photo.MongoDB.Models;
using MongoDB.Driver;

namespace DataAccess.Photo.MongoDB
{
    public class PhotoDbContext 
    {
        private readonly MongoClient _client;

        public IMongoCollection<Photos> PhotosCollection { get; set; }
        public IMongoDatabase Database { get; set; }

        public PhotoDbContext(string connectionString = "mongodb://localhost:27017", string nameDatabase = "photo")
        {
            _client = new MongoClient(connectionString);
            Database = _client.GetDatabase(nameDatabase);
            var collections = Database.ListCollectionNames().ToList();
            List<Task> tasks = new List<Task>();
            if (!collections.Contains("photos"))
            {
                tasks.Add(Task.Run(() => { Database.CreateCollectionAsync("photos"); }));
            }
            if (tasks.Any())
            {
                Task.WaitAll(tasks);
            }
            PhotosCollection = Database.GetCollection<Photos>("photos");
        }
    }
}
