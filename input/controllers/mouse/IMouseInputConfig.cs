using Godot;

namespace Vortico.Input
{
    public interface IMouseInputConfig : IInputConfig
    {
        InputAction GetAction(ButtonList button);
    }
}