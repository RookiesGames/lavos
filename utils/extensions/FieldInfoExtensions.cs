using System.Reflection;

namespace Vortico.Utils.Extensions
{
    public static class FieldInfoExtensions
    {
        public static bool HasCustomAttribute<T>(this FieldInfo info) where T : System.Attribute
        {
            return info.GetCustomAttribute<T>() != null;
        }
    }
}