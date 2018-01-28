using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.EntityLikeFrameworkCore.Annotation
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class CollectionAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public CollectionAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}
