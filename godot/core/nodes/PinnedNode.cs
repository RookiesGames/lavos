using System;
using Godot;
using Lavos.Core.Scene;

namespace Lavos.Core.Nodes
{
    public class PinnedNode : Node
    {
        public override void _Ready()
        {
            NodeTree.PinNode(this.Name, this);
        }
    }
}