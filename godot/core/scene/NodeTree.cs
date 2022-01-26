using System;
using System.Collections.Generic;
using Godot;
using Lavos.Core.Console;
using Lavos.Core.Debug;
using Lavos.Core.Nodes;
using Lavos.Utils.Extensions;

namespace Lavos.Core.Scene
{
    public sealed class NodeTree : Node
    {
        #region Properties

        const string TAG = nameof(NodeTree);

        private static Node _rootNode;
        public static Node RootNode => _rootNode;

        private static Dictionary<string, Node> _pinnedNodes = new Dictionary<string, Node>();

        #endregion


        #region Node

        public override void _Ready()
        {
            _rootNode = GetParent();
            Assert.IsTrue(_rootNode != null, "Root node not found");
        }

        #endregion


        #region Methods

        #region Add

        public static Node AddNode(string name, Node parent)
        {
            Assert.IsFalse(string.IsNullOrEmpty(name), "Name must be given");
            return parent.AddNode<PinnedNode>(name);
        }

        public static T AddNode<T>(Node parent) where T : Node
        {
            return parent.AddNode<T>();
        }

        #endregion Add

        #region Remove

        public static void RemoveNode(Node node)
        {
            node.RemoveSelf();
        }

        #endregion Remove

        #region Pin

        public static void PinNode(string key, Node node)
        {
            if (key.IsNullOrEmpty())
            {
                return;
            }

            if (_pinnedNodes.ContainsKey(key))
            {
                Log.Warn(TAG, $"A node was already pinned under key {key}");
                return;
            }

            _pinnedNodes.Add(key, node);
        }


        public static void UnPinNode(string key)
        {
            if (_pinnedNodes.ContainsKey(key))
            {
                _pinnedNodes.Remove(key);
                return;
            }

            Log.Warn(TAG, $"No pinned node found for key {key}");
        }

        public static Node GetPinnedNode(string key)
        {
            if (_pinnedNodes.ContainsKey(key))
            {
                return _pinnedNodes[key];
            }

            Log.Warn(TAG, $"No pinned node found for key {key}");
            return null;
        }

        #endregion Pin

        #endregion Methods
    }
}