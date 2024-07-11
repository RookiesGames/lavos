using Godot;

namespace Lavos.Dependency;

[GlobalClass]
public partial class Config : Resource
{
    public virtual void Configure(IDependencyBinder binder) { }
    public virtual void Initialize(IDependencyResolver resolver) { }
}
