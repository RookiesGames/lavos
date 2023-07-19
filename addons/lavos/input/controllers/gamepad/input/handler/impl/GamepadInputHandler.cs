using Godot;
using System;
using System.Collections.Generic;

namespace Lavos.Input;

sealed partial class GamepadInputHandler
    : Node
    , IGamepadInputHandler
{
    readonly Dictionary<GamepadDevice, GamepadInputNode> _deviceHandlers = new();
    readonly Dictionary<GamepadDevice, IGamepadInputConfig> _configs = new();
    readonly List<IGamepadInputEventListener> _listeners = new();

    #region IGamepadInputHandler

    public void RegisterListener(IGamepadInputEventListener listener)
    {
        _listeners.PushUnique(listener);
    }

    public void UnregisterListener(IGamepadInputEventListener listener)
    {
        _listeners.Remove(listener);
    }

    #endregion

    #region IInputHandler

    void IInputHandler<IGamepadInputConfig>.EnableHandler(IGamepadInputConfig config)
    {
        EnableHandler(GamepadDevice.GamepadAll, config);
    }

    public void EnableHandler(GamepadDevice device, IGamepadInputConfig config)
    {
        foreach (GamepadDevice value in Enum.GetValues<GamepadDevice>())
        {
            if (value == GamepadDevice.GamepadAll)
            {
                continue;
            }

            var flag = (uint)device & (uint)value;
            if (flag != 0)
            {
                DoEnableHandler(value, config);
            }
        }
    }

    void DoEnableHandler(GamepadDevice device, IGamepadInputConfig config)
    {
        _configs.SetOrAdd(device, config);
        //
        if (!_deviceHandlers.ContainsKey(device))
        {
            var handler = this.AddNode<GamepadInputNode>();
            handler.Gamepad = device;
            _deviceHandlers.SetOrAdd(device, handler);
        }
        _deviceHandlers[device].Config = config;
    }

    #endregion

    public void OnGamepadButtonPressed(GamepadDevice device, InputAction action)
    {
        foreach (var listener in _listeners)
        {
            var flag = (int)(listener.Gamepad & device);
            if (flag == 0) continue;
            //
            var handled = listener.OnGamepadButtonPressed(device, action);
            if (handled) return;
        }
    }

    public void OnGamepadButtonReleased(GamepadDevice device, InputAction action)
    {
        foreach (var listener in _listeners)
        {
            var flag = (int)(listener.Gamepad & device);
            if (flag == 0) continue;
            //
            var handled = listener.OnGamepadButtonReleased(device, action);
            if (handled) return;
        }
    }

    public void OnTriggerValueChanged(GamepadDevice device, InputAction action, float value)
    {
        foreach (var listener in _listeners)
        {
            var flag = (int)(listener.Gamepad & device);
            if (flag == 0) continue;
            //
            var handled = listener.OnTriggerValueChanged(device, action, value);
            if (handled) return;
        }
    }

    public void OnAxisValueChanged(GamepadDevice device, InputAction action, Vector2 value)
    {
        foreach (var listener in _listeners)
        {
            var flag = (int)(listener.Gamepad & device);
            if (flag == 0) continue;
            //
            var handled = listener.OnAxisValueChanged(device, action, value);
            if (handled) return;
        }
    }
}
