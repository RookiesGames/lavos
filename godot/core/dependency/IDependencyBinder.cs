using Godot;

namespace Lavos.Core.Dependency
{
    public interface IDependencyBinder
    {
        void Bind<I, C>() where C : I, new();
        void Lookup<I1, I2>() where I2 : I1;
        void Instance<I, C>(C instance) where C : I;
    }
}