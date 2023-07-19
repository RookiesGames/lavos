using Godot;

namespace Lavos.Dependency;

public interface IDependencyBinder
{
    void Bind<I, C>() where I : IService where C : I, new();
    void Lookup<I1, I2>() where I2 : I1, IService;
    void Instance<I, C>(C instance) where I : IService where C : I;
}