using Godot;
using System;
using System.Collections.Generic;

namespace Lavos.Utils.Extensions
{
    public static class NodeExtensions
    {
        public static T GetSelf<T>(this Node node) where T : Node
        {
            return node.GetNode<T>(".");
        }

        public static bool HasChildren(this Node node)
        {
            return node.GetChildCount() > 0;
        }

        public static void GetNodesInChildren<T>(this Node node, List<T> children) where T : Node
        {
            foreach (Node child in node.GetChildren())
            {
                if (child is T newChild)
                {
                    children.Add(newChild);
                }
                //
                if (child.HasChildren())
                {
                    child.GetNodesInChildren<T>(children);
                }
            }
        }

        public static T GetNodeInChildren<T>(this Node node) where T : Node
        {
            foreach (Node child in node.GetChildren())
            {
                if (child is T foundChild)
                {
                    return foundChild;
                }
                //
                if (child.HasChildren())
                {
                    return child.GetNodeInChildren<T>();
                }
            }

            return null;
        }

        public static T GetNodeInChildrenByName<T>(this Node node, string name) where T : Node
        {
            foreach (Node child in node.GetChildren())
            {
                if (child.Name == name)
                {
                    return (T)child;
                }
                //
                if (child.HasChildren())
                {
                    return child.GetNodeInChildrenByName<T>(name);
                }
            }

            return null;
        }

        public static T AddNode<T>(this Node parent, string name = null) where T : Node
        {
            var node = Activator.CreateInstance<T>();
            node.Name = name.IsNotNullOrEmpty() ? name : typeof(T).Name;
            parent.AddChild(node);
            return node;
        }

        public static void RemoveSelf(this Node node)
        {
            var parent = node.GetParent();
            parent?.RemoveChild(node);
            node.QueueFree();
        }
    }
}