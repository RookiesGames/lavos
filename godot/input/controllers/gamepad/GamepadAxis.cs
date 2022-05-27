using Godot;

namespace Lavos.Input
{
    public enum GamepadAxis
    {
        Unknown,
        //
        LeftStick,
        RightStick,
        //
        LeftTrigger,
        RightTrigger,
    }

    public static class GamepadAxisHelper
    {
        public static GamepadAxis FromJoystickList(int value)
        {
            switch (value)
            {
                case ((int)JoystickList.Axis0): return GamepadAxis.LeftStick;
                case ((int)JoystickList.Axis1): return GamepadAxis.LeftStick;
                //
                case ((int)JoystickList.Axis2): return GamepadAxis.RightStick;
                case ((int)JoystickList.Axis3): return GamepadAxis.RightStick;
                //
                case ((int)JoystickList.Axis6): return GamepadAxis.LeftTrigger;
                case ((int)JoystickList.Axis7): return GamepadAxis.RightTrigger;
                //
                default: return GamepadAxis.Unknown;
            }
        }
    }
}