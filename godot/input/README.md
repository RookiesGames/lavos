# Input

Input module


# Index

* [Config](#config)
* [Input Configurations](#input-configurations)
    * [Keyboard](#keyboard)
        * [Keyboard Configuration](#keyboard-configuration)
        * [Keyboard Handler](#keyboard-handler)
        * [Keyboard Listener](#keyboard-listener)
    * [Mouse](#mouse)
        * [Mouse Configuration](#mouse-configuration)
        * [Mouse Handler](#mouse-handler)
        * [Mouse Listener](#mouse-listener)
    * [Gamepad](#gamepad)
        * []


# Config

The input module must be initialized as part of the boot process. 

The easiest way is to create a resource and attached the [InputConfig](./config/InputConfig.cs) script to it. Add your input resource to your game config list so that it is configured on startup.

Three option are available:
1. `Enable Keyboard` - Allow keyboard input
1. `Enable Mouse` - Allow mouse input
1. `Enable Gamepad` - Allow gamepad input

# Input Configurations

The input system will notify listeners according to configurations that are passed down to it. These configurations can be changed at runtime to adapt to the current gameplay or screen. This way certain input events can be ignored without adding extra complexity to the gameplay logic.

There are three configurations that you can implement; one for each support. 
See [IKeyboardInputConfig](./controllers/keyboard/IKeyboardInputConfig.cs), [IMouseInputConfig](./controllers/mouse/IMouseInputConfig.cs) & [IGamepadInputConfig](./controllers/gamepad/IGamepadInputConfig.cs)

Each support will have its own API that matches the support.

## Keyboard

The keyboard handler will notify any key available on a standard keyboard.
Depending on the manufacturer, some keys might not be supported as they require special software/firmware to be detected by the OS.

### Keyboard Configuration

The keyboard input configuration defines a set of keys it will listen to and a `GetAction` method that converts the input key into an [InputAction](./InputAction.cs)

```cs
using Godot;
using Lavos.Input;
using System.Collections.Generic;

public sealed class KeyboardConfig 
    : IKeyboardInputConfig
{
    public IReadOnlyCollection<KeyList> Keys => _acceptedInput;
    readonly HashSet<KeyList> _acceptedInput = new HashSet<KeyList>()
    {
        KeyList.Up,
        KeyList.Down,
        KeyList.Space
    };

    public InputAction GetAction(KeyList key)
    {
        if (_acceptedInput.Contains(key))
        {
            switch (key)
            {
                case KeyList.Up: return InputAction.Up; // Move forward
                case KeyList.Down: return InputAction.Down; // Move backwards
                case KeyList.Space: return InputAction.South; // Jump
                default: return InputAction.Unknown;
            }
        }

        return InputAction.None;
    }
}
```

### Keyboard Handler

We can enable the keyboard handler passing to it a keyboard configuration.
The [IKeyboardInputHandler](./controllers/keyboard/IKeyboardInputHandler.cs) is a service that needs to be located.

```cs
using Godot;
using Lavos.Input; // Needed for the input classes
using Lavos.Dependency; // Needed for locating services

public class PlayerController
    : Node
{
    IKeyboardInputHandler _keyboardHandler = null;
    KeyboardConfig _config = new KeyboardConfig();

    public override void _EnterTree()
    {
        _keyboardHandler = ServiceLocator.Locate<IKeyboardInputHandler>();
        _keyboardHandler.EnableHandler(_config);
    }

    public override void _ExitTree()
    {
        _keyboardHandler.EnableHandler(null);
    }
}
```

Only one keyboard configuration can be active at a time.

### Keyboard Listener

Implement the [IKeyboardInputListener](./controllers/keyboard/IKeyboardInputListener.cs) interface to later register for keyboard events

```cs
using Godot;
using Lavos.Console; // Needed for logging
using Lavos.Input;
using Lavos.Dependency;

public class PlayerController
    : Node
    , IKeyboardInputListener
{
    const string Tag = nameof(PlayerController);

    #region IKeyboardInputListener

    int IKeyboardInputListener.Priority => 0;

    bool IKeyboardInputListener.OnKeyPressed(InputAction action)
    {
        Log.Debug(Tag, $"Action {action} pressed");
        return true;
    }

    bool IKeyboardInputListener.OnKeyReleased(InputAction action)
    {
        Log.Debug(Tag, $"Action {action} released");
        return true;
    }

    #endregion

    // ...
}
```

To receive keyboard events, register as a listener to [IKeyboardInputHandler](./controllers/keyboard/IKeyboardInputHandler.cs)

```cs
using Godot;
using Lavos.Console;
using Lavos.Input;
using Lavos.Dependency; // Needed for locating services

public class PlayerController
    : Node
    , IKeyboardInputListener
{
    // ...

    IKeyboardInputHandler _keyboardHandler = null;

    public override void _EnterTree()
    {
        _keyboardHandler = ServiceLocator.Locate<IKeyboardInputHandler>();
        // ...
    }

    public override void _ExitTree()
    {
        _keyboardHandler.UnregisterListener(this);
        // ...
    }

    public override void _Ready()
    {
        _keyboardHandler.RegisterListener(this);
    }
}
```

## Mouse

The mouse handler will notify position changes as well as mouse button events. 
Depending on the manufacturer, some keys might not be supported as they require 
special software/firmware to be detected by the OS.

### Mouse Configuration

The mouse input configuration defines a set of mouse buttons it will listen 
to and a `GetAction` method that converts the input button into an 
[InputAction](./InputAction.cs)

```cs
using Godot;
using Lavos.Input;
using System.Collections.Generic;

public class MouseConfig : IMouseInputConfig
{
    IReadOnlyCollection<ButtonList> IMouseInputConfig.Buttons => _buttons;
    readonly HshSet<ButtonList> _buttons = new HashSet<ButtonList>()
    {
        ButtonList.Left,
        ButtonList.Right,
    };

    InputAction IMouseInputConfig.GetAction(ButtonList button)
    {
        if (_buttons.Contains(button) == false)
        {
            // Not in accepted input, ignore
            return InputAction.None;
        }

        switch (button)
        {
            case ButtonList.Left: return InputAction.LeftTrigger; // Main ammo
            case ButtonList.Right: return InputAction.RightTrigger; // Alternative ammo
            default: return InputAction.None;
        }
    }
}
```

### Mouse Handler

We can enable the mouse handler passing to it a mouse configuration.
The [IMouseInputHandler](./controllers/mouse/IMouseInputHandler.cs) 
is a service that needs to be located.

```cs
using Godot;
using Lavos.Input; // Needed for the input classes
using Lavos.Dependency; // Needed for locating services

public class LazerGun
    : Node
{
    IMouseInputHandler _mouseHandler = null;
    MouseConfig _config = new MouseConfig();

    public override void _EnterTree()
    {
        _mouseHandler = ServiceLocator.Locate<IMouseInputHandler>();
        _mouseHandler.EnableHandler(_config);
    }

    public override void _ExitTree()
    {
        _mouseHandler.EnableHandler(null);
    }
}
```

Only one mouse configuration can be active at a time.

### Mouse Listener

Implement the [IMouseInputListener](./controllers/mouse/IMouseInputListener.cs) interface to later register for mouse events


```cs
using Godot;
using Lavos.Console; // Needed for logging
using Lavos.Input;
using Lavos.Dependency;

public class LazerGun
    : Node
    , IMouseInputListener
{
    const string Tag = nameof(LazerGun);

    #region IMouseInputListener

    int IMouseInputListener.Priority => 0;

    bool IMouseInputListener.OnMouseButtonPressed(InputAction action)
    {
        Log.Debug(Tag, $"Action {action} pressed");
        return true;
    }

    bool IMouseInputListener.OnMouseButtonReleased(InputAction action)
    {
        Log.Debug(Tag, $"Action {action} released");
        return true;
    }

    bool IMouseInputListener.OnMousePositionChanged(Vector2 position)
    {
        Log.Debug(Tag, $"Mouse moved to {position}");
        return true;
    }

    #endregion
    
    // ...
}
```

To receive mouse events, register as a listener to [IMouseInputHandler](./controllers/mouse/IMouseInputHandler.cs)


```cs
using Godot;
using Lavos.Console;
using Lavos.Input;
using Lavos.Dependency; // Needed for locating services

public class LazerGun
    : Node
    , IMouseInputListener
{
    // ...

    IMouseInputHandler _mouseHandler = null;

    public override void _EnterTree()
    {
        _mouseHandler = ServiceLocator.Locate<IMouseInputHandler>();
        // ...
    }

    public override void _ExitTree()
    {
        _mouseHandler.UnregisterListener(this);
        // ...
    }

    public override void _Ready()
    {
        _mouseHandler.RegisterListener(this);
    }
}
```

## Gamepad



### Gamepad Config


### Gamepad Handler


### Gamepad Listener