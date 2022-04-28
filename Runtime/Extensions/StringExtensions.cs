namespace OcUtility
{
    public static class StringExtensions
    {
        public static bool equal(this string source, string value)
        {
            return string.CompareOrdinal(source, value) == 0;
        }
    }
}