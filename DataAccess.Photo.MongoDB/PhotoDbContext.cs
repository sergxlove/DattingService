using DataAccess.Photo.MongoDB.Models;
using MongoDB.Driver;

namespace DataAccess.Photo.MongoDB
{
    public class PhotoDbContext 
    {
        private readonly IMongoClient _client;

        public IMongoCollection<Photos> PhotosCollection { get; set; }
        public IMongoDatabase Database { get; set; }

        public PhotoDbContext(IMongoClient client, string nameDatabase = "photo")
        {
            _client = client;
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
