using Godot;

namespace Lavos.Input
{
    public enum GamepadButtons
    {
        Unknown,
        //
        South,
        East,
        West,
        North,
        //
        LeftShoulder,
        RightShoulder,
        //
        LeftTrigger,
        RightTrigger,
        //
        LeftStick,
        RightStick,
        //
        Select,
        Start,
        //
        DPadUp,
        DPadDown,
        DPadLeft,
        DPadRight,
    }

    public static class GamepadButtonsHelper
    {
        public static GamepadButtons FromJoystickList(int value)
        {
            switch (value)
            {
                case ((int)JoystickList.Button0): return GamepadButtons.South;
                case ((int)JoystickList.Button1): return GamepadButtons.East;
                case ((int)JoystickList.Button2): return GamepadButtons.West;
                case ((int)JoystickList.Button3): return GamepadButtons.North;
                //
                case ((int)JoystickList.Button4): return GamepadButtons.LeftShoulder;
                case ((int)JoystickList.Button5): return GamepadButtons.RightShoulder;
                //
                case ((int)JoystickList.Button6): return GamepadButtons.LeftTrigger;
                case ((int)JoystickList.Button7): return GamepadButtons.RightTrigger;
                //
                case ((int)JoystickList.Button8): return GamepadButtons.LeftStick;
                case ((int)JoystickList.Button9): return GamepadButtons.RightStick;
                //
                case ((int)JoystickList.Button10): return GamepadButtons.Select;
                case ((int)JoystickList.Button11): return GamepadButtons.Start;
                //
                case ((int)JoystickList.Button12): return GamepadButtons.DPadUp;
                case ((int)JoystickList.Button13): return GamepadButtons.DPadDown;
                case ((int)JoystickList.Button14): return GamepadButtons.DPadLeft;
                case ((int)JoystickList.Button15): return GamepadButtons.DPadRight;
                //
                default: return GamepadButtons.Unknown;
            }
        }
    }
}