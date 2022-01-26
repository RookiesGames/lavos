using System.Collections.Generic;
using Godot;

namespace Lavos.Input
{
    public interface IKeyboardInputConfig : IInputConfig
    {
        IReadOnlyList<KeyList> Keys { get; }
        InputAction GetAction(KeyList key);
    }
}