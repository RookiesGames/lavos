using Godot;
using System.Collections.Generic;

namespace Lavos.Input;

public interface IMouseInputConfig : IInputConfig
{
    InputAction GetAction(MouseButton button);
}
