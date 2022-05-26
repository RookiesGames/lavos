
namespace Lavos.Input
{
    public interface IGamepadInputListener
    {
        int Priority { get; }

        GamepadDevice Gamepad { get; }

        bool OnGamepadButtonPressed(GamepadDevice device, InputAction action);
        bool OnGamepadButtonReleased(GamepadDevice device, InputAction action);

        bool OnAxisValueChanged(GamepadDevice device, InputAction action, float value);
    }
}