using Godot;
using System.Collections.Generic;

namespace Lavos.Input
{
    public interface IMouseInputConfig : IInputConfig
    {
        IReadOnlyCollection<ButtonList> Buttons { get; }
        InputAction GetAction(ButtonList button);
    }
}