using Godot;
using Lavos.Nodes;

namespace Lavos.Core;

public struct LazyNode<T> where T : Node
{
    public LazyNode() : this(OmniNode.Instance) { }
    public LazyNode(Node parent) { _parent = parent; }

    Node _parent = null;
    T _instance;
    public T Node => _instance ??= _parent.GetNodeInTreeByType<T>();
}