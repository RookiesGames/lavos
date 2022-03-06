
using Godot;
using Lavos.Utils.Extensions;

namespace Lavos.Core.Nodes
{
    public class LavosNode : Node
    {
        public T GetSelf<T>() where T : Node
        {
            return NodeExtensions.GetSelf<T>(this);
        }

        public T GetNodeInChildren<T>() where T : Node
        {
            return NodeExtensions.GetNodeInChildren<T>(this);
        }

        public Node GetNodeInChildren(string name)
        {
            return NodeExtensions.GetNodeInChildren(this, name);
        }

        public T AddNode<T>(string name = null) where T : Node
        {
            return NodeExtensions.AddNode<T>(this, name);
        }

        public void RemoveSelf()
        {
            NodeExtensions.RemoveSelf(this);
        }
    }
}