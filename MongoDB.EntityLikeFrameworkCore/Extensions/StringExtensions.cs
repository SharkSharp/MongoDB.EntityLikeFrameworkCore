using System;
using System.Collections.Generic;
using System.Text;

namespace MongoDB.EntityLikeFrameworkCore.Extensions
{
    static class StringExtensions
    {
        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }
    }
}
