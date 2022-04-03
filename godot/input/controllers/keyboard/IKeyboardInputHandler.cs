using System;

namespace Lavos.Input
{
    public interface IKeyboardInputHandler : IInputHandler<IKeyboardInputConfig>
    {
        event Action<InputAction> KeyPressed;
        event Action<InputAction> KeyReleased;
    }
}