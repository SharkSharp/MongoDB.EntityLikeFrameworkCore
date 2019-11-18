using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using System;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MongoDbContextOptions
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public MongoDatabaseSettings Settings { get; set; }
    }
}
