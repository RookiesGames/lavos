using Godot;

namespace Lavos.Input
{
    public enum GamepadList
    {
        None,
        Button0,
        Button1,
        Button2,
        Button3,
    }

    public static class GamepadListHelper
    {
        public static GamepadList FromJoystickList(int value)
        {
            switch (value)
            {
                case ((int)JoystickList.Button0): return GamepadList.Button0;
                case ((int)JoystickList.Button1): return GamepadList.Button1;
                case ((int)JoystickList.Button2): return GamepadList.Button2;
                case ((int)JoystickList.Button3): return GamepadList.Button3;
                default: return GamepadList.None;
            }
        }
    }
}