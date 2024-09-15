using Godot;
using System;

namespace Lavos.Utils.Platform;

public sealed partial class MobileOnly : Node
{
    public override void _Ready()
    {
        if (PlatformUtils.IsMobile)
        {
            this.RemoveSelfDeferred();
        }
        else
        {
            GetParent()?.RemoveSelfDeferred();
        }
    }
}
