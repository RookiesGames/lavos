using System.Collections.Generic;

namespace Lavos.Input
{
    public interface IGamepadInputConfig : IInputConfig
    {
        IReadOnlyCollection<GamepadButtons> Buttons { get; }
        IReadOnlyCollection<GamepadAxis> Axis { get; }

        InputAction GetActionState(GamepadButtons button);
        InputAction GetAxisState(GamepadAxis axis, float pressure);
        InputAction GetTriggerState(GamepadAxis trigger, float pressure);
    }
}