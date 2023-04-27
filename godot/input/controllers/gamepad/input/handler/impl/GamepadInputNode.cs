using Godot;
using System.Collections.Generic;

namespace Lavos.Input;

sealed partial class GamepadInputNode : Node
{
    readonly Dictionary<JoyAxis, float> _joystickValues = new();
    readonly HashSet<JoyButton> _pressedButtons = new();

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
    public IGamepadInputConfig Config;
    bool IsDisabled => Config == null;

    GamepadInputHandler _parent;

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
            var axis = joypadMotion.Axis;
            switch (axis)
            {
                case JoyAxis.TriggerLeft:
                case JoyAxis.TriggerRight:
                    {
                        var action = Config.GetTriggerState(axis, joypadMotion.AxisValue);
                        if (action == InputAction.None)
                        {
                            return;
                        }
                        //
                        _parent.OnTriggerValueChanged(Gamepad, action, joypadMotion.AxisValue);
                        break;
                    }
                default:
                    {
                        _joystickValues.SetOrAdd(joypadMotion.Axis, joypadMotion.AxisValue);
                        //
                        var action = Config.GetAxisState(axis, joypadMotion.AxisValue);
                        if (action == InputAction.None)
                        {
                            return;
                        }
                        //
                        var value = Vector2.Zero;
                        switch (axis)
                        {
                            case JoyAxis.LeftX:
                            case JoyAxis.LeftY:
                                {
                                    value.X = _joystickValues.GetOrDefault(JoyAxis.LeftX, 0f);
                                    value.Y = _joystickValues.GetOrDefault(JoyAxis.LeftY, 0f);
                                    break;
                                }
                            case JoyAxis.RightX:
                            case JoyAxis.RightY:
                                {
                                    value.X = _joystickValues.GetOrDefault(JoyAxis.RightX, 0f);
                                    value.Y = _joystickValues.GetOrDefault(JoyAxis.RightY, 0f);
                                    break;
                                }
                        }
                        //
                        _parent.OnAxisValueChanged(Gamepad, action, value);
                        break;
                    }
            }
        }
        else if (inputEvent is InputEventJoypadButton joypadButton)
        {
            if (joypadButton.Device != DeviceId)
            {
                return;
            }
            //
            var button = joypadButton.ButtonIndex;
            var action = Config.GetActionState(button);
            //
            if (action == InputAction.None)
            {
                return;
            }
            //
            if (joypadButton.Pressed)
            {
                if (!_pressedButtons.Contains(button))
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