using Godot;

namespace Lavos.Input;

sealed partial class DummyGamepadStatusHandler
    : Node
    , IGamepadStatusHandler
{
    #region IGamepadStatusHandler

    void IGamepadStatusHandler.RegisterListener(IGamepadStatusListener listener) { }

    void IGamepadStatusHandler.UnregisterListener(IGamepadStatusListener listener) { }

    bool IGamepadStatusHandler.IsGamepadConnected(GamepadDevice device) { return false; }

    #endregion IGamepadStatusHandler
}