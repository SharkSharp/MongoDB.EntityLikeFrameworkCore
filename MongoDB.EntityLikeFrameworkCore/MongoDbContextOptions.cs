using MongoDB.Driver;
using System;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MongoDbContextOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="settings"></param>
        public MongoDbContextOptions(string connectionString, MongoDatabaseSettings settings = null)
        {
            ConnectionString = connectionString;
            Settings = settings;
        }

        public string ConnectionString { get; }
        public MongoDatabaseSettings Settings { get; }
    }
}
