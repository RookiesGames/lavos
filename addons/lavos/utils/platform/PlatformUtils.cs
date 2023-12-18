namespace Lavos.Utils.Platform;

public static class PlatformUtils
{
    public enum Platform
    {
        Android,
        iOS,
        macOS,
        Windows,
        Linux,
        Unknwon,
    }

    public static bool IsAndroid =>
#if GODOT_ANDROID
        true;
#else
        false;
#endif

    public static bool IsiOS =>
#if GODOT_IOS
        true;
#else
        false;
#endif

    public static bool IsMobile => IsAndroid || IsiOS;

    public static bool IsDesktop =>
#if GODOT_PC
        true;
#else
        false;
#endif

    public static bool IsLinux =>
#if GODOT_X11
        true;
#else
        false;
#endif

    public static bool IsMacOS =>
#if GODOT_OSX
        true;
#else
        false;
#endif

    public static bool IsWindows =>
#if GODOT_UWP
        true;
#else
        false;
#endif

    public static string OS
    {
        get
        {
            if (IsAndroid)
                return "android";
            if (IsiOS)
                return "ios";
            if (IsLinux)
                return "linux";
            if (IsMacOS)
                return "macos";
            if (IsWindows)
                return "windows";
            return "unknown";
        }
    }

    public static string Arch =>
        System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();

    public static Platform CurrentPlatform
    {
        get
        {
            if (IsAndroid)
                return Platform.Android;
            if (IsiOS)
                return Platform.iOS;
            if (IsLinux)
                return Platform.Linux;
            if (IsMacOS)
                return Platform.macOS;
            if (IsWindows)
                return Platform.Windows;
            return Platform.Unknwon;
        }
    }
}
