
namespace Lavos.Utils.Extensions;

public static class TypeExtensions
{
    public static bool IsAssignableTo(this System.Type type, System.Type to)
    {
        return to.IsAssignableFrom(type);
    }
}
