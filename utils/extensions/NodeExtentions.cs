using System;
using Godot;

namespace Vortico.Utils.Extensions
{
    public static class NodeExtensions
    {
        #region Get

        public static T GetSelf<T>(this Node node) where T : Node
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

        public static Node GetNodeInChildren(this Node node, string name)
        {
            foreach (Node n in node.GetChildren())
            {
                if (n.Name == name)
                {
                    return n;
                }
            }

            return null;
        }

        #endregion Get

        #region Add

        public static T AddNode<T>(this Node parent, string name = null) where T : Node
        {
            var node = Activator.CreateInstance<T>();
            node.Name = name.IsNotNullOrEmpty() ? name : typeof(T).ToString();
            parent.AddChild(node);
            return node;
        }

        #endregion Add

        #region Remove

        public static void RemoveSelf(this Node node)
        {
            var parent = node.GetParent();
            parent?.RemoveChild(node);
            node.QueueFree();
        }

        #endregion
    }
}