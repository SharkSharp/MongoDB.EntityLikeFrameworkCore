using System.ComponentModel.DataAnnotations;

namespace MongoDB.EntityLikeFrameworkCore.Annotation
{
    public class MongoIdAttribute : RegularExpressionAttribute
    {
        public MongoIdAttribute() 
            : base("^[0-9a-fA-F]{24}$") { }
    }
}
