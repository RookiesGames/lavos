using Godot;

namespace Vortico.Core.Dependency
{
    public interface IDependencyContainer
    {
        void Bind<I, C>() where C : I;
        void Lookup<I1, I2>() where I2 : I1;
        void Instance<I, C>(C instance) where C : I;
    }
}