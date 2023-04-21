using Godot;

namespace Lavos.Input;

sealed partial class DummyMouseInputHandler
    : Node
    , IMouseInputHandler
{
    #region IKeyboardInputHandler

    public void RegisterListener(IMouseInputListener listener) { }
    public void UnregisterListener(IMouseInputListener listener) { }

    #endregion

    #region IInputHandler

    public void EnableHandler(IMouseInputConfig config) { }

    #endregion IInputHandler
}
