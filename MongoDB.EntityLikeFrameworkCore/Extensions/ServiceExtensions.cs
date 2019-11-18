using Microsoft.Extensions.DependencyInjection;
using MongoDB.EntityLikeFrameworkCore.Core;
using System;

namespace MongoDB.EntityLikeFrameworkCore.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="setupAction"></param>
        /// <param name="builder"></param>
        public static IMongoDbContextBuilder<T> AddMongoDbContext<T>(this IServiceCollection service,
                                                                     Action<MongoDbContextOptions<T>, string> setupAction) where T : MongoContext
        {
           var options = new MongoDbContextOptions<T>();
            setupAction(options, typeof(T).Name.TrimEnd("Context"));

            service.AddSingleton(x => options);
            service.AddScoped<T>();

            return new MongoDbContextBuilder<T>(service);
        }
    }
}
