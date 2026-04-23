using System.Text.RegularExpressions;

namespace Zuhid.Base;

public static class StringExtension
{
    public static string ToSnakeCase(this string str)
    {
        var result = Regex.Replace(str, "([A-Z][a-z]|(?<=[a-z])[^a-z]|(?<=[A-Z])[0-9_])", "_$1").ToLower();
        return result.StartsWith('_') ? result[1..] : result;
    }
}
