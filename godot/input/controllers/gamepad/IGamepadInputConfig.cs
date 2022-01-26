using System.Collections.Generic;
using Godot;

namespace Lavos.Input
{
    public interface IGamepadInputConfig : IInputConfig
    {
        IReadOnlyList<JoystickList> Axis { get; }

        InputActionState GetAction(JoystickList button, float pressure);
        InputActionState GetMotion(JoystickList motion, float value);
    }
}