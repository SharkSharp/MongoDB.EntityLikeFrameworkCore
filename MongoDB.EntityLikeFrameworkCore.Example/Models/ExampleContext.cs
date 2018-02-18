using MongoDB.Driver;
using MongoDB.EntityLikeFrameworkCore.Annotation;

namespace MongoDB.EntityLikeFrameworkCore.Example.Models
{
    [Database("Different")]
    public class ExampleContext : MongoContext
    {
        public ExampleContext(MongoDbContextOptions<ExampleContext> options)
            : base(options) { }

        [Collection("user")]
        public IMongoCollection<User> Users { get => GetCollection<User>(); }
    }
}
