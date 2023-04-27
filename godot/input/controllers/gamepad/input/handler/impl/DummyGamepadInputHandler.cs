using Godot;

namespace Lavos.Input;

sealed partial class DummyGamepadInputHandler
    : Node
    , IGamepadInputHandler
{
    #region IKeyboardInputHandler

    public void RegisterListener(IGamepadInputEventListener listener) { }
    public void UnregisterListener(IGamepadInputEventListener listener) { }

    #endregion

    #region IInputHandler

    void IInputHandler<IGamepadInputConfig>.EnableHandler(IGamepadInputConfig config) { }
    public void EnableHandler(GamepadDevice device, IGamepadInputConfig config) { }

    #endregion IInputHandler
}