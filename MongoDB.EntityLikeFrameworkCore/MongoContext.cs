using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Annotation;
using MongoDB.EntityLikeFrameworkCore.Extensions;
using System.Reflection;

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
            client = new MongoClient(options.ConnectionString);
            database = GetDatabase(options);
        }

        public IMongoDatabase Database { get => database; }

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

            return database.GetCollection<T>(name);
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
                if (myType.GetCustomAttribute(typeof(DatabaseAttribute)) is DatabaseAttribute attribute)
                    return attribute.Name;
                else
                    return myType.Name.TrimEnd("Context");
            }
        }
    }
}
