
namespace Lavos.Utils.Platform
{
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
    }
}