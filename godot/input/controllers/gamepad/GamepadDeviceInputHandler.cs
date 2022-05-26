using Godot;
using Lavos.Utils.Extensions;
using System.Collections.Generic;

namespace Lavos.Input
{
    sealed class GamepadDeviceInputHandler
        : Node
    {
        readonly HashSet<GamepadList> _pressedButtons = new HashSet<GamepadList>();

        int DeviceId = -1;
        GamepadDevice _gamepad = GamepadDevice.GamepadNone;
        public GamepadDevice Gamepad
        {
            set
            {
                _gamepad = value;
                DeviceId = GamepadDeviceHelper.ToId(_gamepad);
            }
            get => _gamepad;
        }
        public IGamepadInputConfig Config = null;
        bool IsDisabled => Config == null;

        GamepadInputHandler _parent = null;


        public override void _Ready()
        {
            _parent = this.GetNodeInParent<GamepadInputHandler>();
        }

        public override void _Input(InputEvent inputEvent)
        {
            if (IsDisabled)
            {
                return;
            }

            if (inputEvent is InputEventJoypadButton joypadButton)
            {
                if (joypadButton.Device != DeviceId)
                {
                    return;
                }
                //
                var button = GamepadListHelper.FromJoystickList(joypadButton.ButtonIndex);
                var actionState = Config.GetActionState(button, joypadButton.Pressure);
                //
                if (actionState.Action == InputAction.None)
                {
                    return;
                }
                //
                var pressed = joypadButton.Pressed && actionState.Pressed;
                if (pressed)
                {
                    if (_pressedButtons.Contains(button) == false)
                    {
                        _pressedButtons.Add(button);
                        _parent.OnGamepadButtonPressed(Gamepad, actionState.Action);
                    }
                }
                else
                {
                    if (_pressedButtons.Contains(button))
                    {
                        _pressedButtons.Remove(button);
                        _parent.OnGamepadButtonReleased(Gamepad, actionState.Action);
                    }
                }
            }
            else if (inputEvent is InputEventJoypadMotion joypadMotion)
            {
                if (joypadMotion.Device != DeviceId)
                {
                    return;
                }
                //
                var axis = (JoystickList)joypadMotion.Axis;
                var actionState = Config.GetMotionState(axis, joypadMotion.AxisValue);
                //
                if (actionState.Action == InputAction.None)
                {
                    return;
                }
                //
                _parent.OnAxisValueChanged(Gamepad, actionState.Action, joypadMotion.AxisValue);
            }
        }
    }
}