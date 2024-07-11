
namespace Lavos.Input;

public interface IKeyboardInputListener
{
    bool OnKeyPressed(InputAction action) => false;
    bool OnKeyReleased(InputAction action) => false;
}
