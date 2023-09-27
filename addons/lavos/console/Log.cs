using Godot;
using System.Diagnostics;

namespace Lavos.Console;

public static class Log
{
    #region Debug

    [Conditional("DEBUG")]
    public static void Debug<T>(string tag, T message)
    {
        GD.Print($"[Lavos] | [DEBUG] {tag} | {message}");
    }

    [Conditional("DEBUG")]
    public static void Debug<T>(T message)
    {
        Debug<T>("Lavos", message);
    }

    #endregion Debug

    #region Info

    public static void Info<T>(string tag, T message)
    {
        GD.Print($"[Lavos] | [INFO] {tag} | {message}");
    }

    public static void Info<T>(T message)
    {
        Info<T>("Lavos", message);
    }

    #endregion Info

    #region Warn

    public static void Warn<T>(string tag, T message)
    {
        GD.PushWarning($"[Lavos] | [WARNING] {tag} | {message}");
    }

    public static void Warn<T>(T message)
    {
        Warn<T>("Lavos", message);
    }

    #endregion Warn

    #region Error

    public static void Error<T>(string tag, T message)
    {
        GD.Print($"[Lavos] | [ERROR] {tag} | {message}");
    }

    public static void Error<T>(T message)
    {
        Error<T>("Lavos", message);
    }

    #endregion Error
}