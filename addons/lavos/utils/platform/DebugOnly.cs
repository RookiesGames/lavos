using Godot;

namespace Lavos.Utils.Platform;

public sealed partial class DebugOnly : Node
{
    public override void _Ready()
    {
#if DEBUG
        this
#else
        GetParent()
#endif
            .RemoveSelfDeferred();
    }
}