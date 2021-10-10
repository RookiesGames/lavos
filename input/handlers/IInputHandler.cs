using Vortico.Input.Config;
using System;

namespace Vortico.Input.Handlers
{
    public interface IInputHandler<C>
    {
        event Action<InputAction> onInputActionPressed;
        event Action<InputAction> onInputActionReleased;

        void EnableHandler(C config);
        void DisableHandler();
    }
}