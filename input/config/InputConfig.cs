using Godot;
using Vortico.Core.Dependency;

namespace Vortico.Input
{
    public sealed class InputConfig : Config
    {
        [Export] bool EnableKeyboard;
        [Export] bool EnableMouse;
        [Export] bool EnableGamepad;

        public override void Configure(IDependencyContainer container)
        {
            if (EnableKeyboard)
            {
                container.Bind<IKeyboardInputHandler, KeyboardInputHandler>();
            }
            if (EnableMouse)
            {
                container.Bind<IMouseInputHandler, MouseInputHandler>();
            }
            if (EnableGamepad)
            {
                container.Bind<IGamepadInputHandler, GamepadInputHandler>();
            }
        }
    }
}