using System;

namespace Lavos.Input
{
    public interface IKeyboardInputHandler
        : IInputHandler<IKeyboardInputConfig>
    {
        void RegisterListener(IKeyboardInputListener listener);
        void UnregisterListener(IKeyboardInputListener listener);
    }
}