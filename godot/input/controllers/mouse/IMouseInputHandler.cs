using System;
using Godot;
using Lavos.Input;

namespace Lavos.Input
{
    public interface IMouseInputHandler : IInputHandler<IMouseInputConfig>
    {
        event Action<Vector2> onMouseMotion;
    }
}