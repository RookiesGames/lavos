using Godot;
using Lavos.Debug;
using Lavos.Utils.Extensions;
using System.Collections.Generic;
using System;

namespace Lavos.Input
{
    sealed class KeyboardInputHandler
        : Node
        , IKeyboardInputHandler
    {
        #region Members

        IKeyboardInputConfig _config;
        readonly HashSet<KeyList> _pressedKeys = new HashSet<KeyList>();
        readonly List<IKeyboardInputListener> _listeners = new List<IKeyboardInputListener>();

        #endregion


        #region Properties

        bool IsDisabled => _config == null;

        #endregion


        #region IKeyboardInputHandler

        public void RegisterListener(IKeyboardInputListener listener)
        {
            _listeners.InsertUnique(listener.Priority, listener);
        }

        public void UnregisterListener(IKeyboardInputListener listener)
        {
            _listeners.Remove(listener);
        }

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
                        OnKeyPressed(action);
                    }
                }
                else
                {
                    // Remove cache to avoid repetition
                    if (_pressedKeys.Contains(key))
                    {
                        _pressedKeys.Remove(key);
                        OnKeyReleased(action);
                    }
                }
            }
        }

        void OnKeyPressed(InputAction action)
        {
            foreach (var l in _listeners)
            {
                var handled = l.OnKeyPressed(action);
                if (handled)
                {
                    return;
                }
            }
        }

        void OnKeyReleased(InputAction action)
        {
            foreach (var l in _listeners)
            {
                var handled = l.OnKeyReleased(action);
                if (handled)
                {
                    return;
                }
            }
        }

        #endregion
    }
}