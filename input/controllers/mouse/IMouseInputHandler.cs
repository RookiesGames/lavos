using System;
using Godot;
using Vortico.Input;

namespace Vortico.Input
{
    public interface IMouseInputHandler : IInputHandler<IMouseInputConfig>
    {
        event Action<Vector2> onMouseMotion;
    }
}