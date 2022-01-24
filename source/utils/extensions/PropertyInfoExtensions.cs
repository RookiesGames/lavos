using System.Reflection;

namespace Vortico.Utils.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this PropertyInfo info) where T : System.Attribute
        {
            return info.GetCustomAttribute<T>() != null;
        }
    }
}