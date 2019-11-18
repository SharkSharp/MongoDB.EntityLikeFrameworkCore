using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Extensions;
using System;

namespace MongoDB.EntityLikeFrameworkCore
{
    public delegate string OptionStringBuilder(IConfiguration configuration, string contextName);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoDbContextOptions<T> : MongoDbContextOptions where T : MongoContext
    {
        public MongoDbContextOptions(string connectionString,
                                     string databaseName = null,
                                     MongoDatabaseSettings settings = null)
            : base(connectionString, databaseName, settings) { }

        public MongoDbContextOptions(IConfiguration configuration,
                                     OptionStringBuilder connectionStringBuilder,
                                     OptionStringBuilder databaseNameBuilder = null,
                                     MongoDatabaseSettings settings = null)
            : base(connectionStringBuilder(configuration, typeof(T).Name.TrimEnd("Context")),
                   databaseNameBuilder?.Invoke(configuration, typeof(T).Name.TrimEnd("Context")), settings) { }
    }
}
