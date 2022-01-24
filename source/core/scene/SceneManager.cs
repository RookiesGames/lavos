using System;
using System.Threading.Tasks;
using Godot;
using Vortico.Core.Debug;

namespace Vortico.Core.Scene
{
    public sealed class SceneManager : Node
    {
        #region Members

        private static Node _rootNode;

        #endregion


        #region Node

        public override void _Ready()
        {
            _rootNode = this;
        }

        #endregion


        #region LoadScene

        public static async Task<PackedScene> LoadScene(string path)
        {
            var scene = await Task.Factory.StartNew<PackedScene>(() => GD.Load<PackedScene>(path));
            return scene;
        }

        #endregion


        #region ChangeScene

        public static void ChangeScene(PackedScene scene)
        {
            EmptyScene();
            AddScene(scene, _rootNode);
        }

        private static void EmptyScene()
        {
            var children = _rootNode.GetChildren();
            foreach (Node child in children)
            {
                _rootNode.RemoveChild(child);
                child.QueueFree();
            }
        }

        #endregion

        #region Add

        public static Node AddScene(PackedScene scene)
        {
            return AddScene(scene, _rootNode);
        }

        public static Node AddScene(PackedScene scene, Node parent)
        {
            var node = scene.Instance();
            parent.AddChild(node, true);
            return node;
        }

        #endregion
    }
}