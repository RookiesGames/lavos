
using Godot;
using Lavos.Utils.Extensions;

namespace Lavos.Core.Nodes
{
    public class LavosNode : Node
    {
        public T GetSelf<T>() where T : Node
        {
            return this.GetSelf<T>();
        }

        public T GetNodeInChildren<T>() where T : Node
        {
            return this.GetNodeInChildren<T>();
        }

        public Node GetNodeInChildren(string name)
        {
            return this.GetNodeInChildren(name);
        }

        public T AddNode<T>(string name) where T : Node
        {
            return this.AddNode<T>(name);
        }

        public void RemoveSelf()
        {
            this.RemoveSelf();
        }
    }
}