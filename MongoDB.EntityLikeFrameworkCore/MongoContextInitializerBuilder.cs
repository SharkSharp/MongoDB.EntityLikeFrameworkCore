using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    static class MongoContextInitializerBuilder
    {
        static Dictionary<Type, MongoContextInitializer> initializers;

        /// <summary>
        /// 
        /// </summary>
        static MongoContextInitializerBuilder()
        {
            initializers = new Dictionary<Type, MongoContextInitializer>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="options"></param>
        public static void AddInitializer<T>(MongoDbContextOptions<T> options) where T : MongoContext
        {
            initializers.Add(typeof(T), new MongoContextInitializer(options));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MongoContextInitializer Build(MongoDbContextOptions options)
        {
            if (initializers.ContainsKey(options.ContextType))
            {
                return initializers[options.ContextType];
            }
            else
            {
                throw new ArgumentException("Requested initializer not instanciated.");
            }
        }
    }
}
