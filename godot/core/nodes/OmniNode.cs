
using Godot;

namespace Lavos.Core.Nodes
{
    public sealed class OmniNode : Node
    {
        static OmniNode _node;

        public static OmniNode Singleton => _node;

        public override void _Ready()
        {
            _node = this;
        }
    }
}