using System;
using Lavos.Input;

namespace Lavos.Input
{
    public interface IInputHandler<C> where C : IInputConfig
    {
        event Action<InputAction> onInputActionPressed;
        event Action<InputAction> onInputActionReleased;

        void EnableHandler(C config);
        void DisableHandler();
    }
}