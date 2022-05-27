using Godot;
using Lavos.Utils.Extensions;
using System.Collections.Generic;

namespace Lavos.Input
{
    sealed class DummyGamepadHandler
        : Node
        , IGamepadHandler
    {
        #region IGamepadHandler

        void IGamepadHandler.RegisterListener(IGamepadListener listener) { }

        void IGamepadHandler.UnregisterListener(IGamepadListener listener) { }

        #endregion IGamepadHandler
    }
}