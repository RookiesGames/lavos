using Godot;

namespace Lavos.Core.Dependency
{
    public interface IDependencyResolver
    {
        bool Resolve<T>();
    }
}