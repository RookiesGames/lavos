using Godot;
using Lavos.Utils.Extensions;

namespace Lavos.Utils.Platform
{
    public sealed class DesktopOnly : Node
    {
        public override void _EnterTree()
        {
#if !GODOT_PC
            GetParent().RemoveSelf();
#endif
        }
    }
}