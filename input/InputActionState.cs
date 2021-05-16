
namespace Vortico.Input
{
    public class InputActionState
    {
        public InputAction Action { get; private set; }
        public bool Pressed { get; private set; }

        public InputActionState(InputAction action, bool pressed)
        {
            Action = action;
            Pressed = pressed;
        }
    }
}