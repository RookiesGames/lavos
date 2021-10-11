using System;
using Godot;
using Vortico.Core.Debug;
using Vortico.Core.Dependency;
using Vortico.Input;
using Vortico.Utils.Extensions;

namespace Vortico.Input
{
    sealed class KeyboardInputHandler : Node, IKeyboardInputHandler
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

        public void EnableHandler(IKeyboardInputConfig config)
        {
            Assert.IsFalse(config == null, $"Passed null config to {nameof(KeyboardInputHandler)}");
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
            ServiceLocator.Register<IKeyboardInputHandler, KeyboardInputHandler>(this);
        }

        public override void _Process(float delta)
        {
            if (IsEnabled.IsFalse())
            {
                return;
            }

            var keys = _config.Keys;
            foreach (var key in keys)
            {
                var pressed = Godot.Input.IsKeyPressed((int)key);
                var action = _config.GetAction(key);
                if (pressed)
                {
                    onInputActionPressed?.Invoke(action);
                }
                else
                {
                    onInputActionReleased?.Invoke(action);
                }
            }
        }

        #endregion
    }
}