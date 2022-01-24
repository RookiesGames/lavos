using System;
using Godot;
using Vortico.Core.Scene;

namespace Vortico.Core.Nodes
{
    public class PinnedNode : Node
    {
        public override void _Ready()
        {
            NodeTree.PinNode(this.Name, this);
        }
    }
}