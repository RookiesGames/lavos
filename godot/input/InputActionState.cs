
namespace Lavos.Input
{
    public sealed class InputActionState
    {
        public sealed class Definition
        {
            public InputAction Action = InputAction.None;
            public bool Pressed = false;
            public float Pressure = 0f;
        }

        Definition _definition = null;

        public InputAction Action => _definition.Action;
        public bool Pressed => _definition.Pressed;
        public float Pressure => _definition.Pressure;


        public InputActionState(Definition definition)
        {
            _definition = definition;
        }
    }
}