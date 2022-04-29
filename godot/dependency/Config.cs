using Godot;

namespace Lavos.Dependency
{
    public abstract class Config : Resource
    {
        public abstract void Configure(IDependencyBinder binder);
        public abstract void Initialize(IDependencyResolver resolver);
    }
}