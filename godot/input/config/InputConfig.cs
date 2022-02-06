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

        public override void Configure(IDependencyBinder binder)
        {
            var parent = NodeTree.AddNode("Input", NodeTree.RootNode);

            if (EnableKeyboard)
            {
                var handler = NodeTree.AddNode<KeyboardInputHandler>(parent);
                binder.Instance<IKeyboardInputHandler, KeyboardInputHandler>(handler);
            }
            if (EnableMouse)
            {
                var handler = NodeTree.AddNode<MouseInputHandler>(parent);
                binder.Instance<IMouseInputHandler, MouseInputHandler>(handler);
            }
            if (EnableGamepad)
            {
                var handler = NodeTree.AddNode<GamepadInputHandler>(parent);
                binder.Instance<IGamepadInputHandler, GamepadInputHandler>(handler);
            }
        }

        public override void Initialize(IDependencyResolver resolver)
        {
        }
    }
}