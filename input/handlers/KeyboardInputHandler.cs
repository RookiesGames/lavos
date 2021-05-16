using Godot;
using Vortico.Core.Console;
using Vortico.Core.Debug;
using Vortico.Core.Dependency;
using Vortico.Input.Config;
using Vortico.Utils.Extensions;
using System;

namespace Vortico.Input.Handlers
{
    public sealed class KeyboardInputHandler : Node, IKeyboardInputHandler
    {
        #region Members

        private IKeyboardInputConfig _config;

        #endregion


        #region Properties

        private bool IsEnabled => _config != null;

        #endregion


        #region IInputHandler

        public event Action<InputAction> onInputActionPressed;
        public event Action<InputAction> onInputActionReleased;

        public void EnableHandler(IInputConfig config)
        {
            Assert.IsFalse(config == null, $"Passed null config to {nameof(KeyboardInputHandler)}");
            if (config is IKeyboardInputConfig keyboardConfig)
            {
                _config = keyboardConfig;
            }
            else
            {
                Log.Error(nameof(KeyboardInputHandler), $"{nameof(KeyboardInputHandler)} expect config of type {nameof(IKeyboardInputConfig)}");
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
            ServiceLocator.Register<IKeyboardInputHandler, KeyboardInputHandler>(this);
        }

        public override void _Input(InputEvent @event)
        {
            if (IsEnabled.IsFalse())
            {
                return;
            }

            if (@event is InputEventKey eventKey)
            {
                var action = _config.GetAction((KeyList)eventKey.Scancode);
                if (action != InputAction.None)
                {
                    if (eventKey.IsPressed())
                    {
                        onInputActionPressed?.Invoke(action);
                    }
                    else
                    {
                        onInputActionReleased?.Invoke(action);
                    }
                }
            }

            if (@event is InputEventGesture gesture)
            {
            }
        }

        #endregion
    }
}