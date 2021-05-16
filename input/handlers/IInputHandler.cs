using Vortico.Input.Config;
using System;

namespace Vortico.Input.Handlers
{
    public interface IInputHandler
    {
        event Action<InputAction> onInputActionPressed;
        event Action<InputAction> onInputActionReleased;

        void EnableHandler(IInputConfig config);
        void DisableHandler();
    }
}