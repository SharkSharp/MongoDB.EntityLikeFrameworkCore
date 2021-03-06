﻿using Microsoft.Extensions.Logging;
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
        public IClientSessionHandle StartSession(ClientSessionOptions options = null, CancellationToken cancellationToken = default)
        {
            return client.StartSession(options, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IClientSessionHandle> StartSessionAsync(ClientSessionOptions options = null, CancellationToken cancellationToken = default)
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
            var type = typeof(T);

            if (type.GetCustomAttribute(typeof(GenericModelAttribute)) is GenericModelAttribute genericModelAttr &&
                type.IsGenericType && type.GenericTypeArguments.Length == 1)
                type = type.GenericTypeArguments[0];

            if (type.GetCustomAttribute(typeof(CollectionAttribute)) is CollectionAttribute collectionAttr)
                name = collectionAttr.Name;
            else
                name = type.Name;

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

                if (!string.IsNullOrEmpty(options.DatabaseName))
                    return options.DatabaseName;
                else if (myType.GetCustomAttribute(typeof(DatabaseAttribute)) is DatabaseAttribute attribute)
                    return attribute.Name;
                else
                    return myType.Name.TrimEnd("Context");
            }
        }
    }
}
