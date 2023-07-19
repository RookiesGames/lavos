using Godot;

namespace Lavos.Utils;

public partial class NodeSingleton<T> : Node
{
    static T _instance;
    public static T Instance
    {
        get => _instance;
        protected set => _instance = value;
    }
}
