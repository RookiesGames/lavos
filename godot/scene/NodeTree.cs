using System;
using System.Collections.Generic;
using Godot;
using Lavos.Console;
using Lavos.Utils.Extensions;

namespace Lavos.Scene
{
    public sealed class NodeTree : Node
    {
        #region Properties

        const string TAG = nameof(NodeTree);

        private static Dictionary<string, Node> _pinnedNodes = new Dictionary<string, Node>();
        private static Dictionary<string, Node> PinnedNodes => _pinnedNodes;

        private static NodeTree _node;
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

        public static void PinNode(string key, Node node)
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

        public static void PinNode<T>(T node) where T : Node
        {
            var key = typeof(T).Name;
            PinNode(key, node);
        }


        public static void UnpinNode(string key)
        {
            if (PinnedNodes.ContainsKey(key))
            {
                PinnedNodes.Remove(key);
                return;
            }

            Log.Warn(TAG, $"No pinned node found for key {key}");
        }

        public static void UnpinNode<T>()
        {
            var key = typeof(T).Name;
            UnpinNode(key);
        }

        public static Node GetPinnedNode(string key)
        {
            return GetPinnedNode<Node>(key);
        }

        public static T GetPinnedNode<T>(string key) where T : Node
        {
            if (PinnedNodes.ContainsKey(key))
            {
                return (T)PinnedNodes[key];
            }

            Log.Warn(TAG, $"No pinned node found for key {key}");
            return null;
        }

        public static T GetPinnedNode<T>() where T : Node
        {
            var key = typeof(T).Name;
            return GetPinnedNode<T>(key);
        }

        #endregion Methods
    }
}