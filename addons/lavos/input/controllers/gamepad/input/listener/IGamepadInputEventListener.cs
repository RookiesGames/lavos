using Godot;

namespace Lavos.Input;

public interface IGamepadInputEventListener
{
    int Priority { get; }

    GamepadDevice Gamepad { get; }

    bool OnGamepadButtonPressed(GamepadDevice device, InputAction action);
    bool OnGamepadButtonReleased(GamepadDevice device, InputAction action);

    bool OnTriggerValueChanged(GamepadDevice device, InputAction action, float value);
    bool OnAxisValueChanged(GamepadDevice device, InputAction action, Vector2 value);
}