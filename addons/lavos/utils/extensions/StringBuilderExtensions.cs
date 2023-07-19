using System.Text;

namespace Lavos.Utils.Extensions;

public static class StringBuilderExtensions
{
    public static StringBuilder AppendTab(this StringBuilder sb) => sb.Append('\t');
    public static StringBuilder AppendNewLine(this StringBuilder sb) => sb.Append('\n');
}
