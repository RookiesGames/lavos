using System;
using Godot;
using Lavos.Core.Console;
using Lavos.Core.Debug;
using Lavos.Core.Dependency;
using Lavos.Input;
using Lavos.Utils.Extensions;

namespace Lavos.Input
{
    sealed class MouseInputHandler : Node, IMouseInputHandler
    {
        #region Members

        IMouseInputConfig _config;

        #endregion


        #region Properties

        private bool IsEnabled => _config != null;

        #endregion


        #region IInputHandler

        public event Action<InputAction> onInputActionPressed;
        public event Action<InputAction> onInputActionReleased;
        public event Action<Vector2> onMouseMotion;

        public void EnableHandler(IMouseInputConfig config)
        {
            Assert.IsFalse(config == null, $"Passed null config to {nameof(MouseInputHandler)}");
            _config = config;
        }

        public void DisableHandler()
        {
            _config = null;
        }

        #endregion


        #region Node

        public override void _Input(InputEvent inputEvent)
        {
            if (IsEnabled.IsFalse())
            {
                return;
            }

            if (inputEvent is InputEventMouseButton mouseButton)
            {
                var action = _config.GetAction((ButtonList)mouseButton.ButtonIndex);
                if (action != InputAction.None)
                {
                    if (mouseButton.Pressed)
                    {
                        onInputActionPressed?.Invoke(action);
                    }
                    else
                    {
                        onInputActionReleased?.Invoke(action);
                    }
                }
            }
            else if (inputEvent is InputEventMouseMotion mouseMotion)
            {
                onMouseMotion?.Invoke(mouseMotion.Position);
            }
        }

        #endregion
    }

}