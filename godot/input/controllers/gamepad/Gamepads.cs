using System;

namespace Lavos.Input;

[Flags]
public enum GamepadDevice
{
    GamepadNone = 1 << 0,
    Gamepad1 = 1 << 1,
    Gamepad2 = 1 << 2,
    Gamepad3 = 1 << 3,
    Gamepad4 = 1 << 4,
    Gamepad5 = 1 << 5,
    Gamepad6 = 1 << 6,
    Gamepad7 = 1 << 7,
    Gamepad8 = 1 << 8,
    GamepadAll = 0xFFFFFF
}

public static class GamepadDeviceHelper
{
    public static GamepadDevice FromId(int id)
    {
        switch (id)
        {
            case 0: return GamepadDevice.Gamepad1;
            case 1: return GamepadDevice.Gamepad2;
            case 2: return GamepadDevice.Gamepad3;
            case 3: return GamepadDevice.Gamepad4;
            case 4: return GamepadDevice.Gamepad5;
            case 5: return GamepadDevice.Gamepad6;
            case 6: return GamepadDevice.Gamepad7;
            case 7: return GamepadDevice.Gamepad8;
            default: return GamepadDevice.GamepadNone;
        }
    }

    public static int ToId(GamepadDevice device)
    {
        switch (device)
        {
            case GamepadDevice.Gamepad1: return 0;
            case GamepadDevice.Gamepad2: return 1;
            case GamepadDevice.Gamepad3: return 2;
            case GamepadDevice.Gamepad4: return 3;
            case GamepadDevice.Gamepad5: return 4;
            case GamepadDevice.Gamepad6: return 5;
            case GamepadDevice.Gamepad7: return 6;
            case GamepadDevice.Gamepad8: return 7;
            default: return -1;
        }
    }
}
