using System.Collections.Generic;
using Godot;

namespace Lavos.Input
{
    public interface IGamepadInputConfig : IInputConfig
    {
        GamepadDevice Device { get; }

        IReadOnlyCollection<GamepadList> Buttons { get; }
        IReadOnlyCollection<JoystickList> Axis { get; }

        InputActionState GetActionState(GamepadList button, float pressure);
        InputActionState GetMotionState(JoystickList motion, float value);
    }
}