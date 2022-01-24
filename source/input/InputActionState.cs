
namespace Vortico.Input
{
    public class InputActionState
    {
        public InputAction Action { get; set; }
        public bool Pressed { get; set; }

        public InputActionState()
        {
            Action = InputAction.None;
            Pressed = false;
        }
    }
}