
namespace Lavos.Input
{
    public interface IKeyboardInputListener
    {
        int Priority { get; }

        bool OnKeyPressed(InputAction action);
        bool OnKeyReleased(InputAction action);
    }
}