using Godot;
using Lavos.Utils.Extensions;
using System.Collections.Generic;

namespace Lavos.Input
{
    sealed partial class DummyGamepadStatusHandler
        : Node
        , IGamepadStatusHandler
    {
        #region IGamepadStatusHandler

        void IGamepadStatusHandler.RegisterListener(IGamepadStatusListener listener) { }

        void IGamepadStatusHandler.UnregisterListener(IGamepadStatusListener listener) { }

        bool IGamepadStatusHandler.IsGamepadConnected(Lavos.Input.GamepadDevice device) { return false; }

        #endregion IGamepadStatusHandler
    }
}