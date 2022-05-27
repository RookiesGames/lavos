using Godot;
using Lavos.Utils.Extensions;
using System.Collections.Generic;

namespace Lavos.Input
{
    sealed class GamepadHandler
        : Node
        , IGamepadHandler
    {
        readonly List<IGamepadListener> _listeners = new List<IGamepadListener>();

        const float WAIT_TIME = 0.25f;
        float _timer = 0f;

        readonly List<GamepadDevice> _connectedDevices = new List<GamepadDevice>();
        readonly Dictionary<GamepadDevice, bool> _gamepadsState = new Dictionary<GamepadDevice, bool>()
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

        #region IGamepadHandler

        void IGamepadHandler.RegisterListener(IGamepadListener listener)
        {
            _listeners.PushUnique(listener);
        }

        void IGamepadHandler.UnregisterListener(Lavos.Input.IGamepadListener listener)
        {
            _listeners.Remove(listener);
        }

        #endregion IGamepadHandler


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
            var keys = new List<GamepadDevice>(_gamepadsState.Keys);
            foreach (var device in keys)
            {
                var connected = _gamepadsState[device];
                //
                if (_connectedDevices.Contains(device))
                {
                    if (connected == false)
                    {
                        _gamepadsState[device] = true;
                        OnGamepadConnected(device);
                    }
                }
                else
                {
                    if (connected)
                    {
                        _gamepadsState[device] = false;
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