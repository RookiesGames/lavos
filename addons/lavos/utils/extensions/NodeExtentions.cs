using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lavos.Utils.Extensions;

public static class NodeExtensions
{
    public static T GetSelf<T>(this Node node) where T : Node
    {
        return node.GetNode<T>(".");
    }

    public static T GetNodeInParent<T>(this Node node) where T : Node
    {
        var parent = node.GetParent();
        if (parent == null)
        {
            Assert.Fail($"Parent of type {typeof(T)} was not found");
            return null;
        }
        //
        if (parent is T t)
        {
            return t;
        }

        return parent.GetNodeInParent<T>();
    }

    public static bool HasChildren(this Node node)
    {
        return node.GetChildCount() > 0;
    }

    #region GetChildren

    public static List<T> GetChildren<T>(this Node node) where T : Node
    {
        List<T> array = [];
        node.GetChildren(array);
        return array;
    }

    public static void GetChildren<T>(this Node node, ICollection<T> children) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T newChild)
            {
                children.Add(newChild);
            }
        }
    }

    #endregion GetChildren

    #region GetChildrenInTree

    public static void GetChildrenInTreeByType<T>(this Node node, ICollection<T> children) where T : Node
    {
        node.DoGetChildrenInTree((child) => child is T, children);
    }

    public static void GetChildrenInTreeByName<T>(this Node node, string name, ICollection<T> children) where T : Node
    {
        node.DoGetChildrenInTree((child) => child.Name == name && child is T, children);
    }

    static void DoGetChildrenInTree<T>(this Node node, Func<Node, bool> predicate, ICollection<T> children) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (predicate(child))
            {
                children.Add(child as T);
            }
            //
            if (child.HasChildren())
            {
                child.DoGetChildrenInTree(predicate, children);
            }
        }
    }

    #endregion GetChildrenInTree

    #region GetNodeInTree

    public static T GetNodeInTreeByType<T>(this Node node) where T : Node
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
                var value = child.GetNodeInTreeByType<T>();
                if (value != null)
                {
                    return value;
                }
            }
        }
        //
        return null;
    }

    public static T GetNodeInTreeByName<T>(this Node node, string name) where T : Node
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
                var value = child.GetNodeInTreeByName<T>(name);
                if (value != null)
                {
                    return value;
                }
            }
        }
        //
        return null;
    }

    #endregion GetNodeInTree

    #region GetNodeInChildren

    public static T GetNodeInChildrenByType<T>(this Node node) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T foundChild)
            {
                return foundChild;
            }
        }

        Assert.Fail($"Node of type {typeof(T)} was not found");
        return null;
    }

    public static T GetNodeInChildrenByName<T>(this Node node, string name) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child.Name == name && child is T foundChild)
            {
                return foundChild;
            }
        }

        Assert.Fail($"Node \"{name}\" was not found");
        return null;
    }

    #endregion GetNodeInChildren

    #region AddNode

    public static T AddNode<T>(this Node parent, string name = null) where T : Node
    {
        var node = Activator.CreateInstance<T>();
        node.Name = name.IsNotNullOrEmpty() ? name : typeof(T).Name;
        parent.AddChild(node);
        return node;
    }

    public static T AddNode<T>(this Node parent, params object[] args) where T : Node
    {
        var node = (T)Activator.CreateInstance(typeof(T), args);
        node.Name = typeof(T).Name;
        parent.AddChild(node);
        return node;
    }

    public static T AddNode<T>(this Node parent, PackedScene prefab) where T : Node
    {
        var node = prefab.Instantiate<T>();
        parent.AddChild(node);
        return node;
    }

    #endregion AddNode

    #region Remove

    public static void RemoveSelf(this Node node)
    {
        var parent = node.GetParent();
        parent?.RemoveChild(node);
        node.QueueFree();
    }

    public static void RemoveSelfDeferred(this Node node)
    {
        var parent = node.GetParent();
        parent?.CallDeferred("remove_child", node);
        node.QueueFree();
    }

    public static async Task RemoveSelfAsync(this Node node, int delay)
    {
        await Task.Delay(delay);
        node.RemoveSelf();
    }

    public static async Task RemoveSelfAsync(this Node node, Func<bool> fn)
    {
        await Task.Run(async () =>
        {
            while (!fn())
            {
                await Task.Delay(60);
            }
        });
        node.RemoveSelf();
    }

    public static void RemoveChildren(this Node node)
    {
        var children = node.GetChildren();
        foreach (Node child in children)
        {
            child.RemoveSelf();
        }
    }

    #endregion Remove
}