using Godot;
using System.Collections.Generic;

namespace Lavos.Input
{
    public interface IKeyboardInputConfig : IInputConfig
    {
        IReadOnlyCollection<KeyList> Keys { get; }
        InputAction GetAction(KeyList key);
    }
}