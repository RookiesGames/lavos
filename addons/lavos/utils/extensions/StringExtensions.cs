using System.Text;

namespace Lavos.Utils.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string str)
    {
        return string.IsNullOrEmpty(str);
    }

    public static bool IsNotNullOrEmpty(this string str)
    {
        return !string.IsNullOrEmpty(str);
    }

    public static string Repeat(this string str, int number)
    {
        var sb = new StringBuilder();
        for (var idx = 0; idx < number; ++idx)
        {
            sb.Append(str);
        }
        return sb.ToString();
    }
}
