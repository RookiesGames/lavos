using Godot;
using Lavos.Nodes;

namespace Lavos.Core;

public struct LazyNode<T> where T : Node
{
    T _instance;
    public T Node => _instance ??= OmniNode.Instance.GetNodeInChildrenByType<T>();
}