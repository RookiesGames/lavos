using Godot;

namespace Lavos.Utils.Platform;

public sealed partial class DesktopOnly : Node
{
    public override void _Ready()
    {
#if GODOT_PC
        this
#else
            GetParent()
#endif
            .RemoveSelfDeferred();
    }
}
