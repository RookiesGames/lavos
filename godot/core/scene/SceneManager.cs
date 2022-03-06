using System;
using System.Threading.Tasks;
using Godot;
using Lavos.Debug;
using Lavos.Core.Scene;

namespace Lavos.Core.Scene
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


        #region Methods

        public static async Task<PackedScene> LoadScene(string path)
        {
            var scene = await Task.Factory.StartNew<PackedScene>(() => GD.Load<PackedScene>(path));
            return scene;
        }

        public static void ChangeScene(PackedScene scene)
        {
            NodeTree.Singleton.CleanTree();
            AddScene(scene);
        }

        public static Node AddScene(PackedScene scene)
        {
            Assert.IsTrue(scene != null, $"Scene {scene.ResourceName} is nil");
            var node = scene.Instance();
            NodeTree.Singleton.AddChild(node);
            return node;
        }

        #endregion Methods
    }
}