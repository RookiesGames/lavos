using Godot;
using Lavos.Utils.Extensions;

namespace Lavos.Utils.Platform
{
    public sealed class DesktopOnly : Node
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
}