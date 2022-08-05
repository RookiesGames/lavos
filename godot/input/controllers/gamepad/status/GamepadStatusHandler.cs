using Godot;
using Lavos.Utils.Extensions;
using System.Collections.Generic;

namespace Lavos.Input
{
    sealed class GamepadStatusHandler
        : Node
        , IGamepadStatusHandler
    {
        readonly List<IGamepadStatusListener> _listeners = new List<IGamepadStatusListener>();

        const float WAIT_TIME = 0.25f;
        float _timer = WAIT_TIME;

        readonly List<GamepadDevice> _connectedDevices = new List<GamepadDevice>();
        readonly Dictionary<GamepadDevice, bool> _devicesState = new Dictionary<GamepadDevice, bool>()
        {
            { GamepadDevice.Gamepad1, false},
            { GamepadDevice.Gamepad2, false},
            { GamepadDevice.Gamepad3, false},
            { GamepadDevice.Gamepad4, false},
            { GamepadDevice.Gamepad5, false},
            { GamepadDevice.Gamepad6, false},
            { GamepadDevice.Gamepad7, false},
            { GamepadDevice.Gamepad8, false},
        };

        #region IGamepadStatusHandler

        void IGamepadStatusHandler.RegisterListener(IGamepadStatusListener listener)
        {
            _listeners.PushUnique(listener);
        }

        void IGamepadStatusHandler.UnregisterListener(IGamepadStatusListener listener)
        {
            _listeners.Remove(listener);
        }

        bool IGamepadStatusHandler.IsGamepadConnected(GamepadDevice device)
        {
            return _devicesState[device];
        }

        #endregion IGamepadStatusHandler


        public override void _Process(float delta)
        {
            _timer += delta;
            if (_timer >= WAIT_TIME)
            {
                _timer -= WAIT_TIME;
                UpdateDevices();
            }
        }

        void UpdateDevices()
        {
            var joypadIds = Godot.Input.GetConnectedJoypads();
            //
            _connectedDevices.Clear();
            foreach (int id in joypadIds)
            {
                _connectedDevices.Add(GamepadDeviceHelper.FromId(id));
            }
            //
            var keys = new List<GamepadDevice>(_devicesState.Keys);
            foreach (var device in keys)
            {
                var connected = _devicesState[device];
                //
                if (_connectedDevices.Contains(device))
                {
                    if (connected == false)
                    {
                        _devicesState[device] = true;
                        OnGamepadConnected(device);
                    }
                }
                else
                {
                    if (connected)
                    {
                        _devicesState[device] = false;
                        OnGamepadDisconnected(device);
                    }
                }
            }
        }

        void OnGamepadConnected(GamepadDevice device)
        {
            foreach (var listener in _listeners)
            {
                listener.OnGamepadConnected(device);
            }
        }

        void OnGamepadDisconnected(GamepadDevice device)
        {
            foreach (var listener in _listeners)
            {
                listener.OnGamepadDisconnected(device);
            }
        }
    }
}