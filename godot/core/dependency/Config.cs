using Godot;

namespace Lavos.Core.Dependency
{
    public abstract class Config : Node
    {
        public abstract void Configure(IDependencyBinder binder);
        public abstract void Initialize(IDependencyResolver resolver);
    }
}