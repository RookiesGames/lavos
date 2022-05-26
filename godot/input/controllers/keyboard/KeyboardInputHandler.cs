using Godot;
using Lavos.Debug;
using Lavos.Utils.Extensions;
using System.Collections.Generic;

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
            _config = config;
        }

        #endregion IInputHandler


        #region Node

        public override void _Input(InputEvent @event)
        {
            if (IsDisabled)
            {
                return;
            }
            //
            if (@event is InputEventKey eventKey)
            {
                var key = (KeyList)eventKey.Scancode;
                var action = _config.GetAction(key);
                //
                if (action == InputAction.None)
                {
                    return;
                }
                //
                if (eventKey.Pressed)
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