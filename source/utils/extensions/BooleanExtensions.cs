
namespace Vortico.Utils.Extensions
{
    public static class BooleanExtensions
    {
        public static bool IsTrue(this bool value)
        {
            return value == true;
        }

        public static bool IsFalse(this bool value)
        {
            return value == false;
        }
    }
}