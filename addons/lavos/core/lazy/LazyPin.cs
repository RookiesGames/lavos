using Godot;

namespace Lavos.Core;

public struct LazyPin<T> where T : Node
{
    T _instance;
    public T Pin => _instance ??= NodeTree.GetPinnedNodeByType<T>();
}