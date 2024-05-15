using Godot;

namespace Lavos.Input;

public interface IGamepadInputEventListener
{
    GamepadDevice Gamepad { get; }

    bool OnGamepadButtonPressed(GamepadDevice device, InputAction action) { return false; }
    bool OnGamepadButtonReleased(GamepadDevice device, InputAction action) { return false; }

    bool OnTriggerValueChanged(GamepadDevice device, InputAction action, float value) { return false; }
    bool OnAxisValueChanged(GamepadDevice device, InputAction action, Vector2 value) { return false; }
}