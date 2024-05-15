using Godot;

namespace Lavos.Input;

public interface IGamepadInputConfig : IInputConfig
{
    InputAction GetActionState(JoyButton button) { return InputAction.None; }
    InputAction GetAxisState(JoyAxis axis, float value) { return InputAction.None; }
    InputAction GetTriggerState(JoyAxis trigger, float value) { return InputAction.None; }
}