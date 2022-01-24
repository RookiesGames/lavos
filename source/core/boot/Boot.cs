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
            AddChild(new NodeTree());
            AddChild(new SceneManager());
            //
            CreateConfigs();
            //
            SceneManager.ChangeScene(_scene);
        }

        private void CreateConfigs()
        {
            foreach (PackedScene ps in _configs)
            {
                var node = ps.Instance();
                var config = node.GetSelf<Config>();
                config.Configure();
            }
        }
    }
}