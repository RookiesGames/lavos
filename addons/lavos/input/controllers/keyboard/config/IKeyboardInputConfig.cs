using Godot;

namespace Lavos.Input;

public interface IKeyboardInputConfig : IInputConfig
{
    InputAction GetAction(Key key);
}
