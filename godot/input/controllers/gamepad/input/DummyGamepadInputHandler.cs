using Godot;

namespace Lavos.Input;

sealed partial class DummyGamepadInputHandler
    : Node
    , IGamepadInputHandler
{
    #region IKeyboardInputHandler

    public void RegisterListener(IGamepadInputListener listener) { }
    public void UnregisterListener(IGamepadInputListener listener) { }

    #endregion

    #region IInputHandler

    void IInputHandler<IGamepadInputConfig>.EnableHandler(IGamepadInputConfig config) { }
    public void EnableHandler(GamepadDevice device, IGamepadInputConfig config) { }

    #endregion IInputHandler
}