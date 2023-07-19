using Godot;

namespace Lavos.Dependency;

[GlobalClass]
public abstract partial class Config : Resource
{
    public abstract void Configure(IDependencyBinder binder);
    public abstract void Initialize(IDependencyResolver resolver);
}
