using Godot;

namespace Lavos.Dependency;

public interface IDependencyResolver
{
    T Resolve<T>();
}