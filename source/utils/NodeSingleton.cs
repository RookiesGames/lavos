using Godot;

namespace Vortico.Utils
{
    public class NodeSingleton<T> : Node
    {
        protected static T _instance;
        public static T Instance => _instance;
    }
}