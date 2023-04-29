using Godot;

namespace Lavos.Utils.Lazy;

public sealed class LazyPin<T> where T : Node
{
    T _instance = default;
    public T Pin => _instance ??= NodeTree.GetPinnedNodeByType<T>();
}