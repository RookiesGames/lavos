using Godot;

namespace Lavos.Input
{
    public interface IMouseInputConfig : IInputConfig
    {
        InputAction GetAction(ButtonList button);
    }
}