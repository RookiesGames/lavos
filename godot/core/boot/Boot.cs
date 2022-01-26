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