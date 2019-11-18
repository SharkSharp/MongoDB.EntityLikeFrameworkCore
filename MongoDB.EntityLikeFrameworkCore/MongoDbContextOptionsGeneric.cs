namespace MongoDB.EntityLikeFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MongoDbContextOptions<T> : MongoDbContextOptions where T : MongoContext { }
}
