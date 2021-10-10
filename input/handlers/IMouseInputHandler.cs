using Godot;
using Vortico.Input.Config;
using System;

namespace Vortico.Input.Handlers
{
    public interface IMouseInputHandler : IInputHandler<IMouseInputConfig>
    {
        event Action<Vector2> onMouseMotion;
    }
}