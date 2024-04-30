using Godot;
using Godot.Collections;
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

    public static List<T> GetChildren<[MustBeVariant] T>(this Node node) where T : Node
    {
        List<T> array = [];
        foreach (Node child in node.GetChildren())
        {
            if (child is T newChild)
            {
                array.Add(newChild);
            }
        }
        return array;
    }

    public static void GetChildren<T>(this Node node, HashSet<T> children) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T newChild)
            {
                children.Add(newChild);
            }
        }
    }

    public static void GetChildren<T>(this Node node, List<T> children) where T : Node
    {
        for (var idx = 0; idx < node.GetChildCount(); ++idx)
        {
            if (node.GetChild(idx) is T newChild)
            {
                children.Add(newChild);
            }
        }
    }

    public static void GetChildrenRecursively<T>(this Node node, HashSet<T> children) where T : Node
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
                child.GetChildrenRecursively(children);
            }
        }
    }

    public static T GetNodeInChildrenByType<T>(this Node node) where T : Node
    {
        var value = node.DoGetNodeInChildrenByType<T>(recursive: true);
        Assert.IsTrue(value != null, $"Node of type {typeof(T)} was not found");
        return value;
    }

    public static T GetNodeInDirectChildrenByType<T>(this Node node) where T : Node
    {
        var value = node.DoGetNodeInChildrenByType<T>(recursive: false);
        Assert.IsTrue(value != null, $"Node of type {typeof(T)} was not found");
        return value;
    }

    static T DoGetNodeInChildrenByType<T>(this Node node, bool recursive) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child is T foundChild)
            {
                return foundChild;
            }
            //
            if (recursive && child.HasChildren())
            {
                var value = child.DoGetNodeInChildrenByType<T>(recursive);
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
        var value = node.DoGetNodeInChildrenByName<T>(name, recursive: true);
        Assert.IsTrue(value != null, $"Node \"{name}\" was not found");
        return value;
    }

    public static T GetNodeInDirectChildrenByName<T>(this Node node, string name) where T : Node
    {
        var value = node.DoGetNodeInChildrenByName<T>(name, recursive: false);
        Assert.IsTrue(value != null, $"Node \"{name}\" was not found");
        return value;
    }

    static T DoGetNodeInChildrenByName<T>(this Node node, string name, bool recursive) where T : Node
    {
        foreach (Node child in node.GetChildren())
        {
            if (child.Name == name)
            {
                return (T)child;
            }
            //
            if (recursive && child.HasChildren())
            {
                var value = child.DoGetNodeInChildrenByName<T>(name, recursive);
                if (value != null)
                {
                    return value;
                }
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