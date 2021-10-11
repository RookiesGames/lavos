using Godot;

namespace Vortico.Utils.Extensions
{
    public static class NodeExtensions
    {
        public static T GetNode<T>(this Node node) where T : Node
        {
            return node.GetNode<T>(".");
        }

        public static T GetNodeInChildren<T>(this Node node) where T : Node
        {
            foreach (Node n in node.GetChildren())
            {
                if (n.GetType() == typeof(T))
                {
                    return (T)n;
                }
            }

            return null;
        }
    }
}