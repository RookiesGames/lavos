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
            if (EnableKeyboard)
            { binder.Bind<IKeyboardInputHandler, KeyboardInputHandler>(); }
            else
            { binder.Bind<IKeyboardInputHandler, DummyKeyboardInputHandler>(); }
            //
            if (EnableMouse)
            { binder.Bind<IMouseInputHandler, MouseInputHandler>(); }
            else
            { binder.Bind<IMouseInputHandler, DummyMouseInputHandler>(); }
            //
            if (EnableGamepad)
            { binder.Bind<IGamepadInputHandler, GamepadInputHandler>(); }
            else
            { binder.Bind<IGamepadInputHandler, DummyGamepadInputHandler>(); }
        }

        public override void Initialize(IDependencyResolver resolver)
        {
            if (EnableKeyboard) { resolver.Resolve<IKeyboardInputHandler>(); }
            if (EnableMouse) { resolver.Resolve<IMouseInputHandler>(); }
            if (EnableGamepad) { resolver.Resolve<IGamepadInputHandler>(); }
        }
    }
}