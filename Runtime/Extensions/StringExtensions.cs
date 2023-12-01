using System.Text.RegularExpressions;

namespace OcUtility
{
    public static class StringExtensions
    {
        public static bool equal(this string source, string value)
        {
            return string.CompareOrdinal(source, value) == 0;
        }
        public static string trimAll(this string source)
        {
            return source.Replace(" ", "");
        }
    }
}