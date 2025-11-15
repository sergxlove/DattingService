using StackExchange.Redis;

namespace DataAccess.Decks.Redis
{
    public class DecksDbContext
    {
        private readonly ConnectionMultiplexer _redis;
        public IDatabase _database { get; }

        public DecksDbContext(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            _database = _redis.GetDatabase();
        }

        public void Dispose()
        {
            _redis?.Dispose();
        }
    }
}
