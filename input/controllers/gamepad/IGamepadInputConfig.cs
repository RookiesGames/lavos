using System.Collections.Generic;
using Godot;

namespace Vortico.Input
{
    public interface IGamepadInputConfig : IInputConfig
    {
        IReadOnlyList<JoystickList> Axis { get; }

        InputActionState GetAction(JoystickList button, float pressure);
        InputActionState GetMotion(JoystickList motion, float value);
    }
}