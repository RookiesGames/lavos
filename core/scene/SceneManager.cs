using Godot;
using Vortico.Core.Debug;

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
        public static void LoadScene(PackedScene scene)
        {
            LoadScene(scene.ResourcePath);
        }

        public static void LoadScene(string path)
        {
            CleanupScene();
            AddScene(path, _sceneNode);
        }

        private static void CleanupScene()
        {
            var children = _sceneNode.GetChildren();
            foreach (Node child in children)
            {
                _sceneNode.RemoveChild(child);
                child.Dispose();
            }
        }
        #endregion


        #region AddScene
        public static void AddScene(PackedScene scene, Node parent)
        {
            AddScene(scene.ResourcePath, parent);
        }

        public static void AddScene(string path, Node parent)
        {
            var packedScene = GD.Load<PackedScene>(path);
            var node = packedScene.Instance();
            parent.AddChild(node, true);
        }
        #endregion
    }
}