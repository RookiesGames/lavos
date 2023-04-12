using Godot;
using System.Diagnostics;

namespace Lavos.Console;

public static class Log
{
    #region Debug

    [Conditional("DEBUG")]
    public static void Debug<T>(string tag, T message)
    {
        GD.Print($"[DEBUG] {tag} | {message}");
    }

    [Conditional("DEBUG")]
    public static void Debug<T>(T message)
    {
        Debug<T>("Lavos", message);
    }

    #endregion Debug

    #region Warn

    public static void Warn<T>(string tag, T message)
    {
        GD.PushWarning($"[WARNING] {tag} | {message}");
    }

    public static void Warn<T>(T message)
    {
        Warn<T>("Lavos", message);
    }

    #endregion Warn

    #region Error

    public static void Error<T>(string tag, T message)
    {
        GD.Print($"[ERROR] {tag} | {message}");
    }

    public static void Error<T>(T message)
    {
        Error<T>("Lavos", message);
    }

    #endregion Error
}