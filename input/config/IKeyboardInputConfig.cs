using Godot;

namespace Vortico.Input.Config
{
    public interface IKeyboardInputConfig
    {
        InputAction GetAction(KeyList key);
    }
}