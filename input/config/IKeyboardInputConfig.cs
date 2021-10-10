using Godot;
using System.Collections.Generic;

namespace Vortico.Input.Config
{
    public interface IKeyboardInputConfig
    {
        IReadOnlyList<KeyList> Keys { get; }
        InputAction GetAction(KeyList key);
    }
}