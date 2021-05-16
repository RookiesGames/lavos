using Godot;
using System;

namespace Vortico.Input.Handlers
{
    public interface IMouseInputHandler : IInputHandler
    {
        event Action<Vector2> onMouseMotion;
    }
}