using Lavos.Utils.Extensions;

namespace Lavos.Core.Debug
{
    public static class Assert
    {
        public static void IsTrue(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition.IsTrue(), message);
        }

        public static void IsFalse(bool condition, string message)
        {
            System.Diagnostics.Debug.Assert(condition.IsFalse(), message);
        }

        public static void Fail(string message)
        {
            System.Diagnostics.Debug.Assert(false, message);
        }
    }
}