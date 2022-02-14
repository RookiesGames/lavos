using Godot;

namespace Lavos.Core.Dependency
{
    public interface IDependencyResolver
    {
        T Resolve<T>();
    }
}