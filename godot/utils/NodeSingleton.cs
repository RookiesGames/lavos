using Godot;

namespace Lavos.Utils
{
    public partial class NodeSingleton<T> : Node
    {
        protected static T _instance;
        public static T Instance => _instance;
    }
}