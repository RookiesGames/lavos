using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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

    public static void GetNodesInChildren<T>(this Node node, HashSet<T> children) where T : Node
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
        var value = node.DoGetNodeInChildren<T>();
        Assert.IsTrue(value != null, $"Node of type {typeof(T)} was not found");
        return value;
    }

    static T DoGetNodeInChildren<T>(this Node node) where T : Node
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
                var value = child.DoGetNodeInChildren<T>();
                if (value != null)
                {
                    return value;
                }
            }
        }

        return null;
    }

    public static T GetNodeInChildrenByName<T>(this Node node, string name) where T : Node
    {
        var value = node.DoGetNodeInChildrenByName<T>(name);
        Assert.IsTrue(value != null, $"Node \"{name}\" was not found");
        return value;
    }

    static T DoGetNodeInChildrenByName<T>(this Node node, string name) where T : Node
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
                var value = child.DoGetNodeInChildrenByName<T>(name);
                if (value != null)
                {
                    return value;
                }
            }
        }

        return null;
    }

    public static T AddNode<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] T>(this Node parent, string name = null) where T : Node
    {
        var node = Activator.CreateInstance<T>();
        node.Name = name.IsNotNullOrEmpty() ? name : typeof(T).Name;
        parent.AddChild(node);
        return node;
    }

    public static T AddNode<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this Node parent, params object[] args) where T : Node
    {
        var node = (T)Activator.CreateInstance(typeof(T), args);
        node.Name = typeof(T).Name;
        parent.AddChild(node);
        return node;
    }

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
}