using System.Collections.Generic;
using Godot;
using Lavos.Core.Dependency;
using Lavos.Core.Scene;
using Lavos.Utils.Extensions;

namespace Lavos.Core.Boot
{
    sealed class Boot : Node
    {
        [Export] PackedScene _scene;
        [Export] List<PackedScene> _configs;

        List<Config> _configNodes;

        public override void _Ready()
        {
            AddChild(new NodeTree());
            AddChild(new SceneManager());
            //
            if (_configs != null && _configs.Count > 0)
            {
                CreateConfigs();
                InitializeConfigs();
                CleanConfigs();
            }
            //
            SceneManager.ChangeScene(_scene);
        }

        private void CreateConfigs()
        {
            _configNodes = new List<Config>(_configs.Count);
            foreach (PackedScene ps in _configs)
            {
                var node = ps.Instance();
                var config = node.GetSelf<Config>();
                config.Configure(DependencyContainer.Singleton);
                //
                _configNodes.Add(config);
            }
        }

        private void InitializeConfigs()
        {
            foreach (Config config in _configNodes)
            {
                config.Initialize(DependencyContainer.Singleton);
            }
        }

        private void CleanConfigs()
        {
            foreach (Node node in _configNodes)
            {
                node.RemoveSelf();
            }
            _configNodes.Clear();
        }
    }
}
