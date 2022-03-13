using System.Collections.Generic;
using Godot;
using Lavos.Dependency;
using Lavos.Scene;
using Lavos.Utils.Extensions;


namespace Lavos.Nodes
{
    sealed class OmniNode : Node
    {
        [Export] PackedScene _scene = null;
        [Export] List<PackedScene> _configs = new List<PackedScene>();


        public override void _Ready()
        {
            this.AddNode<DependencyContainer>();
            this.AddNode<ServiceLocator>();
            this.AddNode<NodeTree>();
            this.AddNode<SceneManager>();
            //
            HandleConfigs();
            //
            SceneManager.ChangeScene(_scene);
        }

        private void HandleConfigs()
        {
            if (_configs?.Count > 0)
            {
                var configNodes = new List<Config>(_configs.Count);
                CreateConfigs(configNodes);
                InitializeConfigs(configNodes);
                CleanConfigs(configNodes);
                configNodes.Clear();
            }
        }

        private void CreateConfigs(List<Config> nodes)
        {
            foreach (var ps in _configs)
            {
                var node = ps.Instance();
                var config = node.GetSelf<Config>();
                config.Configure(DependencyContainer.Singleton);
                //
                nodes.Add(config);
            }
        }

        private void InitializeConfigs(List<Config> nodes)
        {
            foreach (var config in nodes)
            {
                config.Initialize(DependencyContainer.Singleton);
            }
        }

        private void CleanConfigs(List<Config> nodes)
        {
            foreach (var node in nodes)
            {
                node.RemoveSelf();
            }
        }
    }
}
