using Godot;
using System;
using System.Diagnostics;

namespace Lavos.Console;

public static class Log
{
    static String Time => DateTime.Now.ToString("HH:mm:ss");
    #region Debug

    [Conditional("DEBUG")]
    public static void Debug<T>(string tag, T message)
    {
        GD.Print($"{Time} - [Lavos] | [DEBUG] {tag} | {message}");
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
        GD.Print($"{Time} - [Lavos] | [INFO] {tag} | {message}");
    }

    public static void Info<T>(T message)
    {
        Info<T>("Lavos", message);
    }

    #endregion Info

    #region Warn

    public static void Warn<T>(string tag, T message)
    {
        GD.PushWarning($"{Time} - [Lavos] | [WARNING] {tag} | {message}");
    }

    public static void Warn<T>(T message)
    {
        Warn<T>("Lavos", message);
    }

    #endregion Warn

    #region Error

    public static void Error<T>(string tag, T message)
    {
        GD.PrintErr($"{Time} - [Lavos] | [ERROR] {tag} | {message}");
    }

    public static void Error<T>(T message)
    {
        Error<T>("Lavos", message);
    }

    #endregion Error
}