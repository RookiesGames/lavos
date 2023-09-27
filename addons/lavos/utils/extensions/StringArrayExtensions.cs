using System.Text;

namespace Lavos.Utils.Extensions;

public static class StringArrayExtensions
{
    public static string AsString(this string[] array)
    {
        var sb = new StringBuilder();
        foreach (var item in array)
        {
            sb.Append($"{item} - ");
        }
        return sb.ToString();
    }
}