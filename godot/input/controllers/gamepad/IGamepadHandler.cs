using Lavos.Dependency;

namespace Lavos.Input
{
    public interface IGamepadHandler : IService
    {
        void RegisterListener(IGamepadListener listener);
        void UnregisterListener(IGamepadListener listener);
    }
}