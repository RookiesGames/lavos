using Godot;
using Lavos.Core.Dependency;
using Lavos.Core.Scene;

namespace Lavos.Input
{
    public sealed class InputConfig : Config
    {
        [Export] bool EnableKeyboard;
        [Export] bool EnableMouse;
        [Export] bool EnableGamepad;

        public override void Configure()
        {
            var parent = NodeTree.AddNode("Input", NodeTree.RootNode);

            if (EnableKeyboard)
            {
                var handler = NodeTree.AddNode<KeyboardInputHandler>(parent);
                ServiceLocator.Register<IKeyboardInputHandler, KeyboardInputHandler>(handler);
            }
            if (EnableMouse)
            {
                var handler = NodeTree.AddNode<MouseInputHandler>(parent);
                ServiceLocator.Register<IMouseInputHandler, MouseInputHandler>(handler);
            }
            if (EnableGamepad)
            {
                var handler = NodeTree.AddNode<GamepadInputHandler>(parent);
                ServiceLocator.Register<IGamepadInputHandler, GamepadInputHandler>(handler);
            }
        }
    }
}