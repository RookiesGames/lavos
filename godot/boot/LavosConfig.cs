using Godot;
using Lavos.Dependency;
using Lavos.Utils.Threading;
using System.Collections.Generic;

namespace Lavos.Boot
{
    public sealed class LavosConfig : Config
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