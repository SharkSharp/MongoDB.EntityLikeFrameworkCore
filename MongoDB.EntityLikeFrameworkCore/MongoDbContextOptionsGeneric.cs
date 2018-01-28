using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoDbContextOptions<T> : MongoDbContextOptions where T : MongoContext
    {
        public MongoDbContextOptions(string connectionString, MongoDatabaseSettings settings = null)
            : base(typeof(T), connectionString, settings) { }
    }
}
