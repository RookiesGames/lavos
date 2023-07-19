using Godot;
using System.Collections.Generic;

namespace Lavos.Input;

public interface IGamepadInputConfig : IInputConfig
{
    IReadOnlyCollection<JoyButton> Buttons { get; }
    IReadOnlyCollection<JoyAxis> Axis { get; }

    InputAction GetActionState(JoyButton button);
    InputAction GetAxisState(JoyAxis axis, float value);
    InputAction GetTriggerState(JoyAxis trigger, float value);
}