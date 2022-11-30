using System.Collections.Generic;

namespace Lavos.Input
{
    public interface IGamepadInputConfig : IInputConfig
    {
        IReadOnlyCollection<Godot.JoyButton> Buttons { get; }
        IReadOnlyCollection<Godot.JoyAxis> Axis { get; }

        InputAction GetActionState(Godot.JoyButton button);
        InputAction GetAxisState(Godot.JoyAxis axis, float pressure);
        InputAction GetTriggerState(Godot.JoyAxis trigger, float pressure);
    }
}