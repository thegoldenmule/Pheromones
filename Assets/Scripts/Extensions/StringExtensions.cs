public static class StringExtensions
{
    public static string FormatStr(this string @this, params object[] args)
    {
        return string.Format(@this, args);
    }
}