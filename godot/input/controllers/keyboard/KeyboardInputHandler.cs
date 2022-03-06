using System;
using System.Collections.Generic;
using Godot;
using Lavos.Debug;

namespace Lavos.Input
{
    sealed class KeyboardInputHandler
        : Node
        , IKeyboardInputHandler
    {
        #region Members

        private IKeyboardInputConfig _config;
        private HashSet<KeyList> _pressedKeys = new HashSet<KeyList>();

        #endregion


        #region Properties

        private bool IsDisabled => _config == null;

        #endregion


        #region IKeyboardInputHandler

        public event Action<InputAction> onKeyPressed;
        public event Action<InputAction> onKeyReleased;

        #endregion


        #region IInputHandler

        public void EnableHandler(IKeyboardInputConfig config)
        {
            Assert.IsFalse(config == null, $"Passed null config to {nameof(KeyboardInputHandler)}");
            _config = config;
        }

        public void DisableHandler()
        {
            _config = null;
        }

        #endregion IInputHandler


        #region Node

        public override void _Process(float delta)
        {
            if (IsDisabled)
            {
                return;
            }

            var keys = _config.Keys;
            foreach (var key in keys)
            {
                var action = _config.GetAction(key);
                var pressed = Godot.Input.IsKeyPressed((int)key);
                //
                if (pressed)
                {
                    // Cache press to avoid repetition
                    if (_pressedKeys.Contains(key) == false)
                    {
                        _pressedKeys.Add(key);
                        onKeyPressed?.Invoke(action);
                    }
                }
                else
                {
                    // Remove cache to avoid repetition
                    if (_pressedKeys.Contains(key))
                    {
                        _pressedKeys.Remove(key);
                        onKeyReleased?.Invoke(action);
                    }
                }
            }
        }

        #endregion
    }
}