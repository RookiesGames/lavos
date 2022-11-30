using Godot;
using Lavos.Dependency;
using Lavos.Scene;
using Lavos.Utils;
using Lavos.Utils.Extensions;
using System.Collections.Generic;


namespace Lavos.Nodes
{
    public sealed partial class OmniNode : NodeSingleton<OmniNode>
    {
        [Export] PackedScene _scene = null;
        [Export] Config[] _configs = null;


        public override void _EnterTree()
        {
            _instance = this;
        }

        public override void _Ready()
        {
            var container = this.AddNode<DependencyContainer>();
            this.AddNode<ServiceLocator>(container);
            this.AddNode<NodeTree>();
            this.AddNode<SceneManager>();
            //
            HandleConfigs(container);
            //
            SceneManager.ChangeScene(_scene);
        }

        private void HandleConfigs(DependencyContainer container)
        {
            if (_configs?.Length > 0)
            {
                CreateConfigs(container);
                InitializeConfigs(container);
            }
        }

        private void CreateConfigs(DependencyContainer container)
        {
            foreach (var config in _configs)
            {
                config.Configure(container);
            }
        }

        private void InitializeConfigs(DependencyContainer container)
        {
            foreach (var config in _configs)
            {
                config.Initialize(container);
            }
        }

        public static void RequestQuit()
        {
            Instance.GetTree().Quit();
        }
    }
}
