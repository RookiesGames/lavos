using Godot;
using System.Collections.Generic;

namespace Lavos.Scene;

public sealed partial class NodeTree : Node
{
    #region Properties

    const string TAG = nameof(NodeTree);

    static Dictionary<string, Node> _pinnedNodes = new Dictionary<string, Node>();
    static Dictionary<string, Node> PinnedNodes => _pinnedNodes;

    static NodeTree _node;
    public static NodeTree Instance => _node;

    #endregion


    #region Node

    public override void _EnterTree()
    {
        _node = this;
    }

    #endregion


    #region Methods

    public static void CleanTree()
    {
        var children = _node.GetChildren();
        foreach (Node child in children)
        {
            child.RemoveSelf();
        }
    }

    public static void PinNodeByKey(string key, Node node)
    {
        if (key.IsNullOrEmpty())
        {
            return;
        }

        if (PinnedNodes.ContainsKey(key))
        {
            Log.Warn(TAG, $"A node was already pinned under key {key}");
            return;
        }

        PinnedNodes.Add(key, node);
    }

    public static void PinNodeByType<T>(T node) where T : Node
    {
        var key = typeof(T).Name;
        PinNodeByKey(key, node);
    }


    public static void UnpinNodeByKey(string key)
    {
        if (PinnedNodes.ContainsKey(key))
        {
            PinnedNodes.Remove(key);
            return;
        }

        Log.Warn(TAG, $"No pinned node found for key {key}");
    }

    public static void UnpinNodeByType<T>()
    {
        var key = typeof(T).Name;
        UnpinNodeByKey(key);
    }

    public static T GetPinnedNodeByKey<T>(string key) where T : Node
    {
        if (PinnedNodes.ContainsKey(key))
        {
            return (T)PinnedNodes[key];
        }

        Log.Warn(TAG, $"No pinned node found for key {key}");
        return null;
    }

    public static T GetPinnedNodeByType<T>() where T : Node
    {
        var key = typeof(T).Name;
        return GetPinnedNodeByKey<T>(key);
    }

    #endregion Methods
}