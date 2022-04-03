using Godot;
using Lavos.Audio;
using Lavos.Dependency;
using Lavos.Scene;
using Lavos.Utils.Extensions;
using System.Collections.Generic;


namespace Lavos.Nodes
{
    public sealed class OmniNode : Node
    {
        [Export] PackedScene _scene = null;
        [Export] List<PackedScene> _configs = new List<PackedScene>();

        static OmniNode _instance = null;
        public static OmniNode Instance => _instance;


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
            this.AddNode<MasterAudio>();
            this.AddNode<MusicManager>();
            this.AddNode<SoundManager>();
            //
            HandleConfigs(container);
            //
            SceneManager.ChangeScene(_scene);
        }

        private void HandleConfigs(DependencyContainer container)
        {
            if (_configs?.Count > 0)
            {
                var configNodes = new List<Config>(_configs.Count);
                CreateConfigs(configNodes, container);
                InitializeConfigs(configNodes, container);
                CleanConfigs(configNodes);
                configNodes.Clear();
            }
        }

        private void CreateConfigs(List<Config> nodes, DependencyContainer container)
        {
            foreach (var ps in _configs)
            {
                var node = ps.Instance();
                var config = node.GetSelf<Config>();
                config.Configure(container);
                //
                nodes.Add(config);
            }
        }

        private void InitializeConfigs(List<Config> nodes, DependencyContainer container)
        {
            foreach (var config in nodes)
            {
                config.Initialize(container);
            }
        }

        private void CleanConfigs(List<Config> nodes)
        {
            foreach (var node in nodes)
            {
                node.RemoveSelf();
            }
        }

        public static void RequestQuit()
        {
            Instance.GetTree().Notification(MainLoop.NotificationWmQuitRequest);
        }
    }
}
