using Godot;

namespace Lavos.Dependency;

public sealed partial class ConfigList : Config
{
    [Export] Config[] Configs = null;


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