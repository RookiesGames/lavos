using Godot;
using Lavos.Nodes;

namespace Lavos.Core;

public struct LazyNode<T> where T : Node
{
    public LazyNode() : this(OmniNode.Instance) { }
    public LazyNode(Node parent) { _parent = parent; }
    public LazyNode(string name, Node parent = null) : this(parent ?? OmniNode.Instance)
    {
        _name = name;
    }

    Node _parent = null;
    string _name = string.Empty;
    T _instance;
    /// <summary>
    /// Node instance
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns>Returns the instance of the Node</returns>
    public T Node
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            //
            if (string.IsNullOrEmpty(_name))
            {
                return _instance = _parent.GetNodeInTreeByType<T>();
            }
            //
            return _parent.GetNodeInTreeByName<T>(_name);
        }
    }
}