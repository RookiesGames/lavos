using Godot;
using Lavos.Utils.Extensions;

namespace Lavos.Utils.Platform
{
    public sealed class DebugOnly : Node
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
}