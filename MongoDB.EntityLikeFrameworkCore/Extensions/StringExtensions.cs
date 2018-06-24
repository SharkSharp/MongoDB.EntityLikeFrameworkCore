using System.Text.RegularExpressions;

namespace MongoDB.EntityLikeFrameworkCore.Extensions
{
    public static class StringExtensions
    {
        public static string TrimEnd(this string source, string value)
        {
            if (!source.EndsWith(value))
                return source;

            return source.Remove(source.LastIndexOf(value));
        }

        public static bool IsValidId(this string str)
        {
            return !string.IsNullOrEmpty(str) && new Regex("^[0-9a-fA-F]{24}$").IsMatch(str);
        }
    }
}
