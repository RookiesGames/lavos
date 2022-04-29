using Godot;
using System.Collections.Generic;

namespace Lavos.Dependency
{
    public sealed class ConfigList : Config
    {
        [Export] List<Config> Configs = new List<Config>();


        public override void Configure(IDependencyBinder binder)
        {
            foreach (var config in Configs)
            {
                config.Configure(binder);
            }
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            foreach (var config in Configs)
            {
                config.Initialize(resolver);
            }
        }
    }
}