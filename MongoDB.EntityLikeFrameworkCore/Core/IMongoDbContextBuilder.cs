using Microsoft.Extensions.DependencyInjection;

namespace MongoDB.EntityLikeFrameworkCore.Core
{
    public interface IMongoDbContextBuilder<T> where T : MongoContext
    {
        void AddMongoDbBuilder(MongoDbBuilder<T> builder);
    }
}