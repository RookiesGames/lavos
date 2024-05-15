
namespace Lavos.Input;

public interface IKeyboardInputListener
{
    bool OnKeyPressed(InputAction action);
    bool OnKeyReleased(InputAction action);
}
