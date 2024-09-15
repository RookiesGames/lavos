namespace Lavos.Utils;

public static class GodotUtils
{
        public static string Version =>
#if GODOT4_4_OR_GREATER
#       if GODOT4_4_0
                "4.4.0";
#       endif
#elif GODOT4_3_OR_GREATER
#       if GODOT4_3_0
                "4.3.0";
#       endif
#elif GODOT4_2_OR_GREATER
#       if GODOT4_2_2
                "4.2.2";
#       elif GODOT4_2_0
                "4.2.0";
#       endif
#elif GODOT4_1_OR_GREATER
#       if GODOT4_1_1
                "4.1.1";
#       elif GODOT4_1_0
                "4.1.0";
#       endif
#else
#error Version not supported
#endif
}