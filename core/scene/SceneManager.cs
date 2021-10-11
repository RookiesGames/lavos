using System;
using System.Threading.Tasks;
using Godot;
using Vortico.Core.Debug;

namespace Vortico.Core.Scene
{
    public sealed class SceneManager : Node
    {
        #region Members

        private const string SceneRootPath = "/root/Boot";
        private static Node _rootNode;

        #endregion


        #region Node

        public override void _Ready()
        {
            _rootNode = this;
        }

        #endregion


        #region LoadScene

        public static async void LoadScene(string path, Action<PackedScene> completeAction)
        {
            var scene = await LoadSceneAsync(path, completeAction);
            completeAction?.Invoke(scene);
        }

        private static Task<PackedScene> LoadSceneAsync(string path, Action<PackedScene> completeAction)
        {
            var task = Task.Factory.StartNew<PackedScene>(() => GD.Load<PackedScene>(path));
            return task;
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