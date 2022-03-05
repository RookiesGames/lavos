using Godot;
using Lavos.Core.Dependency;
using Lavos.Core.Scene;
using Lavos.Utils.Extensions;

namespace Lavos.Input
{
    public sealed class InputConfig : Config
    {
        [Export] bool EnableKeyboard;
        [Export] bool EnableMouse;
        [Export] bool EnableGamepad;

        public override void Configure(IDependencyBinder binder)
        {
            var parent = NodeTree.Singleton.AddNode("Input");

            if (EnableKeyboard)
            {
                var handler = parent.AddNode<KeyboardInputHandler>();
                binder.Instance<IKeyboardInputHandler, KeyboardInputHandler>(handler);
            }
            if (EnableMouse)
            {
                var handler = parent.AddNode<MouseInputHandler>();
                binder.Instance<IMouseInputHandler, MouseInputHandler>(handler);
            }
            if (EnableGamepad)
            {
                var handler = parent.AddNode<GamepadInputHandler>();
                binder.Instance<IGamepadInputHandler, GamepadInputHandler>(handler);
            }
        }

        public override void Initialize(IDependencyResolver resolver)
        {
        }
    }
}