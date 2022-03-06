using System;
using System.Collections.Generic;
using Godot;
using Lavos.Core.Console;
using Lavos.Debug;
using Lavos.Core.Nodes;
using Lavos.Utils.Extensions;

namespace Lavos.Core.Scene
{
    public sealed class NodeTree : LavosNode
    {
        #region Properties

        const string TAG = nameof(NodeTree);

        private static Dictionary<string, Node> _pinnedNodes = new Dictionary<string, Node>();

        private static NodeTree _node;
        public static NodeTree Singleton => _node;

        #endregion


        #region Node

        public override void _Ready()
        {
            _node = this;
        }

        #endregion


        #region Methods

        public void CleanTree()
        {
            // TODO: Remove all nodes under the tree
            /*
                var children = NodeTree.Singleton.GetChildren();
                foreach (Node child in children)
                {
                    _rootNode.RemoveChild(child);
                    child.QueueFree();
                }
            */
        }

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


        public static void UnpinNode(string key)
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