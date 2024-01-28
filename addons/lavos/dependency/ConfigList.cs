using Godot;
using Godot.Collections;

namespace Lavos.Dependency;

[GlobalClass]
public sealed partial class ConfigList : Config
{
    [Export] Array<Config> Configs;

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