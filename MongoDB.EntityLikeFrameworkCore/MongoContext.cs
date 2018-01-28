using MongoDB.Driver;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MongoContext
    {
        private readonly MongoClient client;
        private readonly IMongoDatabase database;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public MongoContext(MongoDbContextOptions options)
        {
            var initializer = MongoContextInitializerBuilder.Build(options);

            client = new MongoClient(options.ConnectionString);
            database = initializer.GetDatabase(options, client);

            initializer.Initialize(this, options, database);
        }
    }
}
