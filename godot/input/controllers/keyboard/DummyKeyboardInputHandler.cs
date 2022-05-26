using Godot;

namespace Lavos.Input
{
    sealed class DummyKeyboardInputHandler
        : Node
        , IKeyboardInputHandler
    {
        #region IKeyboardInputHandler

        public void RegisterListener(IKeyboardInputListener listener) { }
        public void UnregisterListener(IKeyboardInputListener listener) { }

        #endregion


        #region IInputHandler

        public void EnableHandler(IKeyboardInputConfig config) { }

        #endregion IInputHandler
    }
}