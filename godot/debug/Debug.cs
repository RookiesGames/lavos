
namespace Lavos.Debug
{
    public static class Assert
    {
        public static void IsTrue(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition == true, message);
        }

        public static void IsFalse(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition == false, message);
        }

        public static void Fail(string message)
        {
            System.Diagnostics.Debug.Assert(false, message);
        }
    }
}