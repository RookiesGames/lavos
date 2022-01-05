using Godot;
using Vortico.Core.Dependency;
using Vortico.Core.Scene;

namespace Vortico.Input
{
    public sealed class InputConfig : Config
    {
        [Export] bool EnableKeyboard;
        [Export] bool EnableMouse;
        [Export] bool EnableGamepad;

        public override void Configure(IDependencyContainer container)
        {
            var parent = NodeTree.AddNode("Input", NodeTree.RootNode);

            if (EnableKeyboard)
            {
                var handler = NodeTree.AddNode<KeyboardInputHandler>(parent);
                container.Instance<IKeyboardInputHandler, KeyboardInputHandler>(handler);
            }
            if (EnableMouse)
            {
                var handler = NodeTree.AddNode<MouseInputHandler>(parent);
                container.Instance<IMouseInputHandler, MouseInputHandler>(handler);
            }
            if (EnableGamepad)
            {
                var handler = NodeTree.AddNode<GamepadInputHandler>(parent);
                container.Instance<IGamepadInputHandler, GamepadInputHandler>(handler);
            }
        }
    }
}