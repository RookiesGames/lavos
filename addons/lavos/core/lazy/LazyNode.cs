using Godot;
using Lavos.Scene;

namespace Lavos.Core;

public struct LazyNode<T> where T : Node
{
    public LazyNode(string name, Node parent = null) 
    {
        _parent = parent ?? NodeTree.Instance;
        _name = name;
    }

    Node _parent;
    string _name;
    T _instance;
    public T Node => _instance ??= _parent.GetNodeInChildrenByName<T>(_name);
}