using Godot;
using Lavos.Debug;
using Lavos.Utils.Extensions;
using System.Collections.Generic;
using System;

namespace Lavos.Input
{
    sealed partial class MouseInputHandler : Node, IMouseInputHandler
    {
        #region Members

        IMouseInputConfig _config;
        readonly HashSet<Godot.MouseButton> _pressedButtons = new HashSet<Godot.MouseButton>();
        readonly List<IMouseInputListener> _listeners = new List<IMouseInputListener>();

        #endregion


        #region Properties

        bool IsDisabled => _config == null;

        #endregion


        #region IKeyboardInputHandler

        public void RegisterListener(IMouseInputListener listener)
        {
            _listeners.InsertUnique(listener.Priority, listener);
        }

        public void UnregisterListener(IMouseInputListener listener)
        {
            _listeners.Remove(listener);
        }

        #endregion


        #region IInputHandler

        public void EnableHandler(IMouseInputConfig config)
        {
            _config = config;
        }

        #endregion


        #region Node

        public override void _Input(InputEvent inputEvent)
        {
            if (IsDisabled)
            {
                return;
            }
            //
            if (inputEvent is InputEventMouseButton mouseButton)
            {
                var button = mouseButton.ButtonIndex;
                var action = _config.GetAction(button);
                //
                if (action == InputAction.None)
                {
                    return;
                }
                //
                if (mouseButton.Pressed)
                {
                    // Cache press to avoid repetition
                    if (_pressedButtons.Contains(button) == false)
                    {
                        _pressedButtons.Add(button);
                        OnMouseButtonPressed(action);
                    }
                }
                else
                {
                    // Remove cache to avoid repetition
                    if (_pressedButtons.Contains(button))
                    {
                        _pressedButtons.Remove(button);
                        OnMouseButtonReleased(action);
                    }
                }
            }
            else if (inputEvent is InputEventMouseMotion mouseMotion)
            {
                OnMousePositionChanged(mouseMotion.Position);
            }
        }

        void OnMouseButtonPressed(InputAction action)
        {
            foreach (var listener in _listeners)
            {
                var handled = listener.OnMouseButtonPressed(action);
                if (handled)
                {
                    return;
                }
            }
        }

        void OnMouseButtonReleased(InputAction action)
        {
            foreach (var listener in _listeners)
            {
                var handled = listener.OnMouseButtonReleased(action);
                if (handled)
                {
                    return;
                }
            }
        }

        void OnMousePositionChanged(Vector2 position)
        {
            foreach (var listener in _listeners)
            {
                var handled = listener.OnMousePositionChanged(position);
                if (handled)
                {
                    return;
                }
            }
        }

        #endregion
    }

}