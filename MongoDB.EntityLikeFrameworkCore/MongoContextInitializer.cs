using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Annotation;
using MongoDB.EntityLikeFrameworkCore.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    class MongoContextInitializer
    {
        /// <summary>
        /// 
        /// </summary>
        internal class CollectionInfo
        {
            public readonly string propName;
            public readonly string collectionName;
            public readonly MethodInfo getCollection;

            public CollectionInfo(string propName, string collectionName, MethodInfo getCollection)
            {
                this.propName = propName;
                this.collectionName = collectionName;
                this.getCollection = getCollection;
            }
        }

        readonly string databaseName;
        readonly IEnumerable<CollectionInfo> props; 

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public MongoContextInitializer(MongoDbContextOptions options)
        {
            var client = new MongoClient(options.ConnectionString);

            databaseName = GetDatabaseName(options);
            var database = GetDatabase(options, client);

            var codeCollection = GetCodeCollection(options);

            props = GetCodeCollectionTuples(codeCollection);

            HandleNewCollections(database, codeCollection);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public IMongoDatabase GetDatabase(MongoDbContextOptions options, MongoClient client)
        {
            return client.GetDatabase(databaseName, options.Settings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="options"></param>
        /// <param name="database"></param>
        public void Initialize(MongoContext context, MongoDbContextOptions options, IMongoDatabase database)
        {
            var contextType = options.ContextType;

            foreach (var prop in props)
            {
                contextType.GetProperty(prop.propName).SetValue(context, prop.getCollection.Invoke(database, new object[] { prop.collectionName, null }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="codeCollection"></param>
        private static void HandleNewCollections(IMongoDatabase database, IEnumerable<PropertyInfo> codeCollection)
        {
            var codeCollectionNames = GetCodeCollectionNames(codeCollection);
            var absentCollections = codeCollectionNames.Except(GetDBCollectionNames(database));

            foreach (var collection in absentCollections)
            {
                database.CreateCollection(collection);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static string GetDatabaseName(MongoDbContextOptions options)
        {
            var contextType = options.ContextType;
            DatabaseAttribute attribute;
            if ((attribute = contextType.GetCustomAttribute(typeof(DatabaseAttribute)) as DatabaseAttribute) != null)
            {
                return attribute.Name;
            }
            else
            {
                return contextType.Name.TrimEnd("Context");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> GetCodeCollection(MongoDbContextOptions options)
        {
            return options.ContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                          .Where(x => x.PropertyType.IsGenericType &&
                                 x.PropertyType.GetGenericTypeDefinition() == typeof(IMongoCollection<>).GetGenericTypeDefinition());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetDBCollectionNames(IMongoDatabase database)
        {
            return database.ListCollections().ToList().Select(x => x["name"].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeCollection"></param>
        /// <returns></returns>
        private static IEnumerable<string> GetCodeCollectionNames(IEnumerable<PropertyInfo> codeCollection)
        {
            return codeCollection.Select(x =>
                                 {
                                     CollectionAttribute attribute;
                                     if ((attribute = x.GetCustomAttribute(typeof(CollectionAttribute)) as CollectionAttribute) != null)
                                     {
                                         return attribute.Name;
                                     }
                                     else
                                     {
                                         return x.Name;
                                     }
                                 });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeCollection"></param>
        /// <returns></returns>
        private static IEnumerable<CollectionInfo> GetCodeCollectionTuples(IEnumerable<PropertyInfo> codeCollection)
        {
            return codeCollection.Select(x =>
                                 {
                                     string collectionName;
                                     CollectionAttribute attribute;                                  
                                     if ((attribute = x.GetCustomAttribute(typeof(CollectionAttribute)) as CollectionAttribute) != null)
                                     {
                                         collectionName = attribute.Name;
                                     }
                                     else
                                     {
                                         collectionName = x.Name;
                                     }
                                     var method = typeof(IMongoDatabase)
                                                  .GetMethod("GetCollection")
                                                  .MakeGenericMethod(x.PropertyType.GetGenericArguments()
                                                  .First());

                                     return new CollectionInfo(x.Name, collectionName, method);
                                 });
        }
    }
}
