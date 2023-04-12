
namespace Lavos.Input;

public interface IGamepadStatusListener
{
    void OnGamepadConnected(GamepadDevice device);
    void OnGamepadDisconnected(GamepadDevice device);
}
