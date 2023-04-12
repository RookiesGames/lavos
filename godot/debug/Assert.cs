using System.Diagnostics;

namespace Lavos.Debug;

public static class Assert
{
    [Conditional("DEBUG")]
    public static void IsTrue(bool condition, string message)
    {
        System.Diagnostics.Debug.Assert(condition == true, message);
    }

    [Conditional("DEBUG")]
    public static void IsFalse(bool condition, string message)
    {
        System.Diagnostics.Debug.Assert(condition == false, message);
    }

    [Conditional("DEBUG")]
    public static void Fail(string message)
    {
        System.Diagnostics.Debug.Assert(false, message);
    }
}