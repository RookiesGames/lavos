using Godot;
using Vortico.Core.Debug;
using System;
using System.Threading.Tasks;

namespace Vortico.Core.Scene
{
    public sealed class SceneManager : Node
    {
        #region Members

        private const string SceneRootPath = "/root/Scene";
        private static Node _sceneNode;

        #endregion


        #region Node

        public override void _Ready()
        {
            _sceneNode = GetNode(SceneRootPath);
            Assert.IsFalse(_sceneNode == null, "Missing Scene node");
        }

        #endregion


        #region LoadScene

        public static void LoadScene(string path, Action<PackedScene> completeAction)
        {
            LoadSceneAsync(path, completeAction);
        }

        private static async void LoadSceneAsync(string path, Action<PackedScene> completeAction)
        {
            var task = Task.Factory.StartNew<PackedScene>(() =>
            {
                var packedScene = GD.Load<PackedScene>(path);
                return packedScene;
            });
            await task;
            completeAction?.Invoke(task.Result);
        }

        #endregion


        #region ChangeScene

        public static void ChangeScene(PackedScene scene)
        {
            EmptyScene();
            AddPackedSceneToParent(scene, _sceneNode);
        }

        private static void EmptyScene()
        {
            var children = _sceneNode.GetChildren();
            foreach (Node child in children)
            {
                _sceneNode.RemoveChild(child);
                child.QueueFree();
            }
        }

        #endregion


        #region AddScene

        public static void AddPackedSceneToParent(PackedScene scene, Node parent)
        {
            var node = scene.Instance();
            parent.AddChild(node, true);
        }

        #endregion
    }
}