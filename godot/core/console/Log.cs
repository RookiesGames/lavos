using Godot;
using System.Diagnostics;

namespace Lavos.Core.Console
{
    public static class Log
    {
        [Conditional("DEBUG")]
        public static void Debug(string tag, string message)
        {
            GD.Print($"[DEBUG] {tag} | {message}");
        }

        public static void Warn(string tag, string message)
        {
            GD.PushWarning($"[WARNING] {tag} | {message}");
        }

        public static void Error(string tag, string message)
        {
            GD.Print($"[ERROR] {tag} | {message}");
        }
    }
}