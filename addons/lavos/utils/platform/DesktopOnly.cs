using Godot;

namespace Lavos.Utils.Platform;

public sealed partial class DesktopOnly : Node
{
    public override void _Ready()
    {
        if (PlatformUtils.IsDebug)
        {
            this.RemoveSelfDeferred();
        }
        else
        {
            GetParent()?.RemoveSelfDeferred();
        }
    }
}
