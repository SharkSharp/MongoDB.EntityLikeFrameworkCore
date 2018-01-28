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
        /// <param name="contextType"></param>
        /// <param name="connectionString"></param>
        /// <param name="settings"></param>
        public MongoDbContextOptions(Type contextType, string connectionString, MongoDatabaseSettings settings = null)
        {
            ContextType = contextType;
            ConnectionString = connectionString;
            Settings = settings;
        }

        public Type ContextType { get; }
        public string ConnectionString { get; }
        public MongoDatabaseSettings Settings { get; }
    }
}
