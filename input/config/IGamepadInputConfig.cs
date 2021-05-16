using Godot;

namespace Vortico.Input.Config
{
    public interface IGamepadInputConfig : IInputConfig
    {
        InputActionState GetAction(JoystickList button, float pressure);
        InputActionState GetMotion(JoystickList motion, float value);
    }
}