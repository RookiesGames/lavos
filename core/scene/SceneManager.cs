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
        public static void ChangeScene(PackedScene scene)
        {
            EmptyScene();
            AddPackedSceneToParent(scene, _sceneNode);
        }

        public static void ChangeScene(string path)
        {
            EmptyScene();
            AddScenePathToParent(path, _sceneNode);
        }

        private static void EmptyScene()
        {
            var children = _sceneNode.GetChildren();
            foreach (Node child in children)
            {
                _sceneNode.RemoveChild(child);
                child.Dispose();
            }
        }
        #endregion

        #region LoadScene
        public static PackedScene LoadScene(string path)
        {
            var packedScene = GD.Load<PackedScene>(path);
            return packedScene;
        }

        public static void LoadScene(string path, Action<PackedScene> completeAction)
        {
            LoadSceneAsync(path, completeAction);
        }

        private static async void LoadSceneAsync(string path, Action<PackedScene> completeAction)
        {
            var task = Task.Factory.StartNew<PackedScene>(() => LoadScene(path));
            await task;
            completeAction?.Invoke(task.Result);
        }
        #endregion

        #region AddScene
        public static void AddPackedSceneToParent(PackedScene scene, Node parent)
        {
            var node = InstantiateScene(scene);
            AddNode(node, parent);
        }

        public static void AddScenePathToParent(string path, Node parent)
        {
            var packedScene = LoadScene(path);
            var node = InstantiateScene(packedScene);
            AddNode(node, parent);
        }

        private static Node InstantiateScene(PackedScene scene)
        {
            return scene.Instance();
        }

        private static void AddNode(Node node, Node parent)
        {
            parent.AddChild(node, true);
        }
        #endregion
    }
}