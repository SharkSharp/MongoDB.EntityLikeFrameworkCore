using Microsoft.Extensions.DependencyInjection;

namespace MongoDB.EntityLikeFrameworkCore.Extensions
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        /// <param name="options"></param>
        /// <param name="builder"></param>
        public static void AddMongoDbContext<T>(this IServiceCollection service, MongoDbContextOptions<T> options,
                                                MongoDbBuilder<T> builder = null) where T : MongoContext
        {
            service.AddSingleton(x => options);
            service.AddScoped<T>();

            var context = service.BuildServiceProvider().GetRequiredService<T>();
            builder?.Build(context);
        }
    }
}
