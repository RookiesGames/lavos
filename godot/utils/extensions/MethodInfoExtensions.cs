using System.Reflection;

namespace Lavos.Utils.Extensions
{
    public static class MethodInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this MethodInfo info) where T : System.Attribute
        {
            return info.GetCustomAttribute<T>() != null;
        }
    }
}