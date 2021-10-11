using System.Collections.Generic;
using Godot;
using Vortico.Core.Dependency;
using Vortico.Core.Scene;
using Vortico.Utils.Extensions;

namespace Vortico.Core.Boot
{
    sealed class Boot : Node
    {
        [Export] PackedScene _scene;
        [Export] List<PackedScene> _configs;

        public override void _Ready()
        {
            var container = new DependencyContainer();
            AddChild(container);
            //
            var locator = new ServiceLocator(container);
            AddChild(locator);
            //
            AddChild(new SceneManager());
            //
            CreateConfigs(container);
            //
            SceneManager.ChangeScene(_scene);
        }

        private void CreateConfigs(DependencyContainer container)
        {
            foreach (PackedScene ps in _configs)
            {
                var node = ps.Instance();
                var config = node.GetNode<Config>();
                config.Configure(container);
            }
        }
    }
}