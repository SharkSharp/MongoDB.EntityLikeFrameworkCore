using System;

namespace MongoDB.EntityLikeFrameworkCore.Annotation
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class GenericModelAttribute : Attribute
    {
    }
}
