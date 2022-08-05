using Lavos.Dependency;

namespace Lavos.Input
{
    public interface IGamepadStatusHandler : IService
    {
        void RegisterListener(IGamepadStatusListener listener);
        void UnregisterListener(IGamepadStatusListener listener);

        bool IsGamepadConnected(GamepadDevice device);
    }
}