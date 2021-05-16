using Godot;

namespace Vortico.Input.Config
{
    public interface IMouseInputConfig : IInputConfig
    {
        InputAction GetAction(ButtonList button);
    }
}