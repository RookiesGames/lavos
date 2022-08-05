using Godot;
using Lavos.Utils.Extensions;
using System.Collections.Generic;

namespace Lavos.Input
{
    sealed class GamepadDeviceInputHandler
        : Node
    {
        readonly Dictionary<int, float> _joystickValues = new Dictionary<int, float>();
        readonly HashSet<GamepadButtons> _pressedButtons = new HashSet<GamepadButtons>();

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

            if (inputEvent is InputEventJoypadMotion joypadMotion)
            {
                if (joypadMotion.Device != DeviceId)
                {
                    return;
                }
                //
                var axis = GamepadAxisHelper.FromJoystickList(joypadMotion.Axis);
                //
                if (axis == GamepadAxis.LeftTrigger || axis == GamepadAxis.RightTrigger)
                {
                    var action = Config.GetTriggerState(axis, joypadMotion.AxisValue);
                    if (action == InputAction.None)
                    {
                        return;
                    }
                    //
                    _parent.OnTriggerValueChanged(Gamepad, action, joypadMotion.AxisValue);
                }
                else
                {
                    _joystickValues.SetOrAdd(joypadMotion.Axis, joypadMotion.AxisValue);
                    //
                    var action = Config.GetAxisState(axis, joypadMotion.AxisValue);
                    if (action == InputAction.None)
                    {
                        return;
                    }
                    //
                    var haxis = (axis == GamepadAxis.LeftStick)
                                            ? ((int)JoystickList.Axis0)
                                            : ((int)JoystickList.Axis2);
                    var vaxis = (axis == GamepadAxis.LeftStick)
                                            ? ((int)JoystickList.Axis1)
                                            : ((int)JoystickList.Axis3);
                    //
                    var value = Vector2.Zero;
                    value.x = _joystickValues.GetOrDefault(haxis, 0f);
                    value.y = _joystickValues.GetOrDefault(vaxis, 0f);
                    //
                    _parent.OnAxisValueChanged(Gamepad, action, value);
                }
            }
            else if (inputEvent is InputEventJoypadButton joypadButton)
            {
                if (joypadButton.Device != DeviceId)
                {
                    return;
                }
                //
                var button = GamepadButtonsHelper.FromJoystickList(joypadButton.ButtonIndex);
                var action = Config.GetActionState(button);
                //
                if (action == InputAction.None)
                {
                    return;
                }
                //
                if (joypadButton.Pressed)
                {
                    if (_pressedButtons.Contains(button) == false)
                    {
                        _pressedButtons.Add(button);
                        _parent.OnGamepadButtonPressed(Gamepad, action);
                    }
                }
                else
                {
                    if (_pressedButtons.Contains(button))
                    {
                        _pressedButtons.Remove(button);
                        _parent.OnGamepadButtonReleased(Gamepad, action);
                    }
                }
            }
        }
    }
}