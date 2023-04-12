using Godot;

namespace Lavos.Input;

public interface IMouseInputListener
{
    int Priority { get; }

    bool OnMousePositionChanged(Vector2 position);
    bool OnMouseButtonPressed(InputAction action);
    bool OnMouseButtonReleased(InputAction action);
}
