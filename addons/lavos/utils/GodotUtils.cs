namespace Lavos.Utils;

public static class GodotUtils
{
    public static string Version =>
#if GODOT4_2_0
        "4.2.0";
#elif GODOT4_1_1
        "4.1.1";
#elif GODOT4_1_0
        "4.1.0";
#else
#error Version not supported
#endif
}