using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Annotation;
using MongoDB.EntityLikeFrameworkCore.Extensions;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MongoContext
    {
        private readonly MongoClient client;
        private readonly MongoDbContextOptions options;

        public IMongoDatabase Database { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public MongoContext(MongoDbContextOptions options)
        {
            this.options = options;

            client = new MongoClient(options.ConnectionString);
            Database = GetDatabase(options);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IClientSessionHandle StartSession(ClientSessionOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return client.StartSession(options, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions options = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await client.StartSessionAsync(options, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected IMongoCollection<T> GetCollection<T>()
        {
            string name = "";
            if (typeof(T).GetCustomAttribute(typeof(CollectionAttribute)) is CollectionAttribute attribute)
                name = attribute.Name;
            else
                name = typeof(T).Name;

            return Database.GetCollection<T>(name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private IMongoDatabase GetDatabase(MongoDbContextOptions options)
        {
            return client.GetDatabase(DatabaseName, options.Settings);
        }

        /// <summary>
        /// 
        /// </summary>
        public string DatabaseName
        {
            get
            {
                var myType = GetType();
                var contextTypeName = myType.Name.TrimEnd("Context");
                var envDatabaseName = Environment.GetEnvironmentVariable($"{contextTypeName.ToUpper()}_MONGO_DATABASE_NAME");

                if (!string.IsNullOrEmpty(envDatabaseName))
                    return envDatabaseName;

                if (!string.IsNullOrEmpty(options.DatabaseName))
                    return options.DatabaseName;

                if (myType.GetCustomAttribute(typeof(DatabaseAttribute)) is DatabaseAttribute attribute)
                    return attribute.Name;

                return contextTypeName;
            }
        }
    }
}
