using System;
using Godot;
using Lavos.Scene;

namespace Lavos.Nodes
{
    public class PinnedNode : Node
    {
        public override void _EnterTree()
        {
            NodeTree.PinNode(this.Name, this);
        }
    }
}