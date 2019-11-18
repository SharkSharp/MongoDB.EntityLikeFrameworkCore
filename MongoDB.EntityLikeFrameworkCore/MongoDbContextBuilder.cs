using Microsoft.Extensions.DependencyInjection;
using MongoDB.EntityLikeFrameworkCore.Core;

namespace MongoDB.EntityLikeFrameworkCore
{
    public class MongoDbContextBuilder<T> : IMongoDbContextBuilder<T> where T : MongoContext
    {
        private readonly IServiceCollection service;

        public MongoDbContextBuilder(IServiceCollection service)
        {
            this.service = service;
        }

        public void AddMongoDbBuilder(MongoDbBuilder<T> builder)
        {
            var context = service.BuildServiceProvider().GetRequiredService<T>();
            builder?.Build(context);
        }
    }
}
