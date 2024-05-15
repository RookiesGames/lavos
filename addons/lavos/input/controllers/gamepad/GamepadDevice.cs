using System;

namespace Lavos.Input;

[Flags]
public enum GamepadDevice : uint
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
    GamepadAll = Gamepad1 | Gamepad2 | Gamepad3 | Gamepad4 | Gamepad5 | Gamepad6 | Gamepad7 | Gamepad8,
}

public static class GamepadDeviceHelper
{
    public static GamepadDevice FromId(int id)
    {
        return id switch
        {
            0 => GamepadDevice.Gamepad1,
            1 => GamepadDevice.Gamepad2,
            2 => GamepadDevice.Gamepad3,
            3 => GamepadDevice.Gamepad4,
            4 => GamepadDevice.Gamepad5,
            5 => GamepadDevice.Gamepad6,
            6 => GamepadDevice.Gamepad7,
            7 => GamepadDevice.Gamepad8,
            _ => GamepadDevice.GamepadNone,
        };
    }

    public static int ToId(GamepadDevice device)
    {
        return device switch
        {
            GamepadDevice.Gamepad1 => 0,
            GamepadDevice.Gamepad2 => 1,
            GamepadDevice.Gamepad3 => 2,
            GamepadDevice.Gamepad4 => 3,
            GamepadDevice.Gamepad5 => 4,
            GamepadDevice.Gamepad6 => 5,
            GamepadDevice.Gamepad7 => 6,
            GamepadDevice.Gamepad8 => 7,
            _ => -1,
        };
    }
}
