
namespace Lavos.Input
{
    public interface IGamepadListener
    {
        void OnGamepadConnected(GamepadDevice device);
        void OnGamepadDisconnected(GamepadDevice device);
    }
}