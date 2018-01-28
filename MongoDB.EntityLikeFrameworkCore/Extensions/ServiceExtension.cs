using Microsoft.Extensions.DependencyInjection;

namespace MongoDB.EntityLikeFrameworkCore.Extensions
{
    public static class ServiceExtension
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="options"></param>
        public static void AddMongoDbContext<T>(this IServiceCollection service, MongoDbContextOptions<T> options) where T : MongoContext
        {
            MongoContextInitializerBuilder.AddInitializer(options);
            service.AddSingleton(x => options);
            service.AddScoped<T>();
        }
    }
}
