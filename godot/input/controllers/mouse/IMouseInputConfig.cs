using Godot;
using System.Collections.Generic;

namespace Lavos.Input
{
    public interface IMouseInputConfig : IInputConfig
    {
        IReadOnlyCollection<Godot.MouseButton> Buttons { get; }
        InputAction GetAction(Godot.MouseButton button);
    }
}