﻿using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Annotation;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MongoDbBuilder<T> where T : MongoContext
    {
        private readonly bool resetDatabase;

        /// <summary>
        /// 
        /// </summary>
        public MongoDbBuilder(bool resetDatabase = false)
        {
            this.resetDatabase = resetDatabase;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ModelType"></typeparam>
        /// <param name="collection"></param>
        protected virtual void Seed<ModelType>(IMongoCollection<ModelType> collection) { }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="ModelType"></typeparam>
        /// <param name="collection"></param>
        protected virtual void Index<ModelType>(IMongoCollection<ModelType> collection) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void Seed(T context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        protected virtual void Index(T context) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Build(T context)
        {
            if (resetDatabase)
                context.Database.Client.DropDatabase(context.DatabaseName);

            HandleNewCollections(context, GetAbsentCollectionNames(context));

            Index(context);
            Seed(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="absentCollections"></param>
        private void HandleNewCollections(T context, IEnumerable<PropertyInfo> absentCollections)
        {
            foreach (var collection in absentCollections)
            {
                GetType()
                .GetRuntimeMethods()
                .Where(x => x.Name == "Index")
                .First()
                .MakeGenericMethod(collection.PropertyType.GetGenericArguments().First())
                .Invoke(this, new object[] { collection.GetValue(context) });

                GetType()
                .GetRuntimeMethods()
                .Where(x => x.Name == "Seed")
                .First()
                .MakeGenericMethod(collection.PropertyType.GetGenericArguments().First())
                .Invoke(this, new object[] { collection.GetValue(context) });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetAbsentCollectionNames(T context)
        {
            var codeCollection = GetCodeCollection();
            var dbCollection = GetDBCollectionNames(context);
            return codeCollection.Where(x => !dbCollection.Contains(GetCodeCollectionName(x)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<PropertyInfo> GetCodeCollection()
        {
            return typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public)
                          .Where(x => x.PropertyType.IsGenericType &&
                                 x.PropertyType.GetGenericTypeDefinition() == typeof(IMongoCollection<>).GetGenericTypeDefinition());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private IEnumerable<string> GetDBCollectionNames(T context)
        {
            return context.Database.ListCollections().ToList().Select(x => x["name"].ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="codeCollection"></param>
        /// <returns></returns>
        private string GetCodeCollectionName(PropertyInfo propInfo)
        {
            CollectionAttribute attribute;
            if ((attribute = propInfo.PropertyType.GetGenericArguments().First().GetCustomAttribute(typeof(CollectionAttribute)) as CollectionAttribute) != null)
            {
                return attribute.Name;
            }
            else
            {
                return propInfo.PropertyType.GetGenericArguments().First().Name;
            }

        }
    }
}
