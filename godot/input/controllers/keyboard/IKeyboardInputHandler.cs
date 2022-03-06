using System;

namespace Lavos.Input
{
    public interface IKeyboardInputHandler : IInputHandler<IKeyboardInputConfig>
    {
        event Action<InputAction> onKeyPressed;
        event Action<InputAction> onKeyReleased;
    }
}