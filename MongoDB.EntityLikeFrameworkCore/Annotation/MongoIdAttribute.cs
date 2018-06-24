using System.ComponentModel.DataAnnotations;

namespace Hermes.Web.Attributes
{
    public class MongoIdAttribute : RegularExpressionAttribute
    {
        public MongoIdAttribute() 
            : base("^[0-9a-fA-F]{24}$") { }
    }
}
