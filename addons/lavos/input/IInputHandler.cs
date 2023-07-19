using Lavos.Dependency;

namespace Lavos.Input;

public interface IInputHandler<C>
    : IService
    where C : IInputConfig
{
    void EnableHandler(C config);
}