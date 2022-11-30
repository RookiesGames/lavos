using Godot;
using System.Collections.Generic;

namespace Lavos.Input
{
    public interface IKeyboardInputConfig : IInputConfig
    {
        IReadOnlyCollection<Godot.Key> Keys { get; }
        InputAction GetAction(Godot.Key key);
    }
}