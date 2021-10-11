using System.Collections.Generic;
using Godot;

namespace Vortico.Input
{
    public interface IKeyboardInputConfig : IInputConfig
    {
        IReadOnlyList<KeyList> Keys { get; }
        InputAction GetAction(KeyList key);
    }
}