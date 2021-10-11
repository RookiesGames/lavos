using System;
using Godot;
using Vortico.Core.Debug;
using Vortico.Core.Dependency;
using Vortico.Utils.Extensions;

namespace Vortico.Input
{
    sealed class GamepadInputHandler : Node, IGamepadInputHandler
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

        public void EnableHandler(IGamepadInputConfig config)
        {
            Assert.IsFalse(config == null, $"Passed null config to {nameof(GamepadInputHandler)}");
            _config = config;
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

        public override void _Process(float delta)
        {
            if (IsEnabled.IsFalse())
            {
                return;
            }

            var controls = _config.Axis;
            foreach (var control in controls)
            {
                var value = Godot.Input.GetJoyAxis(0, (int)control);
                var ias = _config.GetMotion(control, value);
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

        public override void _Input(InputEvent inputEvent)
        {
            if (IsEnabled.IsFalse())
            {
                return;
            }

            if (inputEvent is InputEventJoypadButton joypadButton)
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
            else if (inputEvent is InputEventJoypadMotion joypadMotion)
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