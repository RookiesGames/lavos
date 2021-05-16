using Godot;
using Vortico.Core.Console;
using Vortico.Core.Debug;
using Vortico.Core.Dependency;
using Vortico.Input.Config;
using Vortico.Utils.Extensions;
using System;

namespace Vortico.Input.Handlers
{
    public sealed class GamepadInputHandler : Node, IGamepadInputHandler
    {
        #region Member

        private IGamepadInputConfig _config;

        #endregion


        #region Properties

        private bool IsEnabled => _config != null;

        #endregion


        #region IInputHandler

        public event Action<InputAction> onInputActionPressed;
        public event Action<InputAction> onInputActionReleased;

        public void EnableHandler(IInputConfig config)
        {
            Assert.IsFalse(config == null, $"Passed null config to {nameof(GamepadInputHandler)}");
            if (config is IGamepadInputConfig gamepadConfig)
            {
                _config = gamepadConfig;
            }
            else
            {
                Log.Error(nameof(GamepadInputHandler), $"{nameof(GamepadInputHandler)} expect config of type {nameof(IGamepadInputHandler)}");
            }
        }

        public void DisableHandler()
        {
            _config = null;
        }

        #endregion


        #region Node

        public override void _Ready()
        {
            ServiceLocator.Register<IGamepadInputHandler, GamepadInputHandler>(this);
        }

        public override void _Input(InputEvent @event)
        {
            if (IsEnabled.IsFalse())
            {
                return;
            }

            if (@event is InputEventJoypadButton joypadButton)
            {
                var ias = _config.GetAction((JoystickList)joypadButton.ButtonIndex, joypadButton.Pressure);
                if (ias.Action != InputAction.None)
                {
                    if (joypadButton.Pressed && ias.Pressed)
                    {
                        onInputActionPressed?.Invoke(ias.Action);
                    }
                    else if (joypadButton.Pressed.IsFalse() && ias.Pressed.IsFalse())
                    {
                        onInputActionReleased?.Invoke(ias.Action);
                    }
                }
            }
            else if (@event is InputEventJoypadMotion joypadMotion)
            {
                var ias = _config.GetMotion((JoystickList)joypadMotion.Axis, joypadMotion.AxisValue);
                if (ias.Action != InputAction.None)
                {
                    if (ias.Pressed)
                    {
                        onInputActionPressed?.Invoke(ias.Action);
                    }
                    else
                    {
                        onInputActionReleased?.Invoke(ias.Action);
                    }
                }
            }
        }

        #endregion
    }
}