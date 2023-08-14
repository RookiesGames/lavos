
namespace Lavos.Utils.Platform;

public static class PlatformUtils
{
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
}
