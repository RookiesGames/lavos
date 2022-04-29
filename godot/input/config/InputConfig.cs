using Godot;
using Lavos.Dependency;
using Lavos.Nodes;
using Lavos.Utils.Extensions;

namespace Lavos.Input
{
    public sealed class InputConfig : Config
    {
        [Export] bool EnableKeyboard = false;
        [Export] bool EnableMouse = false;
        [Export] bool EnableGamepad = false;

        public override void Configure(IDependencyBinder binder)
        {
            //var parent = OmniNode..Singleton.AddNode("Input");

            if (EnableKeyboard)
            {
                //var handler = parent.AddNode<KeyboardInputHandler>();
                //binder.Instance<IKeyboardInputHandler, KeyboardInputHandler>(handler);
                binder.Bind<IKeyboardInputHandler, KeyboardInputHandler>();
            }
            if (EnableMouse)
            {
                //var handler = parent.AddNode<MouseInputHandler>();
                //binder.Instance<IMouseInputHandler, MouseInputHandler>(handler);
                binder.Bind<IMouseInputHandler, MouseInputHandler>();
            }
            if (EnableGamepad)
            {
                //var handler = parent.AddNode<GamepadInputHandler>();
                //binder.Instance<IGamepadInputHandler, GamepadInputHandler>(handler);
                binder.Bind<IGamepadInputHandler, GamepadInputHandler>();
            }
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            if (EnableKeyboard) { resolver.Resolve<IKeyboardInputHandler>(); }
            if (EnableMouse) { resolver.Resolve<IMouseInputHandler>(); }
            if (EnableGamepad) { resolver.Resolve<IGamepadInputHandler>(); }
        }
    }
}