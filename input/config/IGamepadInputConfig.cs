using Godot;
using System.Collections.Generic;

namespace Vortico.Input.Config
{
    public interface IGamepadInputConfig : IInputConfig
    {
        IReadOnlyList<JoystickList> Axis { get; }

        InputActionState GetAction(JoystickList button, float pressure);
        InputActionState GetMotion(JoystickList motion, float value);
    }
}