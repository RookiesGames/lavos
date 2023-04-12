# Input

Input module

# Index

- [Config](#config)
- [Input Configurations](#input-configurations)
  - [Keyboard](#keyboard)
    - [Keyboard Configuration](#keyboard-configuration)
    - [Keyboard Handler](#keyboard-handler)
    - [Keyboard Listener](#keyboard-listener)
  - [Mouse](#mouse)
    - [Mouse Configuration](#mouse-configuration)
    - [Mouse Handler](#mouse-handler)
    - [Mouse Listener](#mouse-listener)
  - [Gamepad](#gamepad)
    - [Gamepad Configuration](#gamepad-configuration)
    - [Gamepad Input Handler](#gamepad-input-handler)
    - [Gamepad Input Listener](#gamepad-input-listener)
    - [Gamepad Status Changes](#gamepad-status-changes)

# Config

The input module must be initialized as part of the boot process.

The easiest way is to create a resource and attached the
[InputConfig](./config/InputConfig.cs) script to it. Add your input resource to
your [game config](../dependency/README.md) list so that it is configured on
startup.

Three option are available:

1. `Enable Keyboard` - Allow keyboard input
1. `Enable Mouse` - Allow mouse input
1. `Enable Gamepad` - Allow gamepad input

# Input Configurations

The input system will notify listeners according to configurations that are
passed down to it. These configurations can be changed at runtime to adapt to
the current gameplay or screen. This way certain input events can be ignored
without adding extra complexity to the gameplay logic.

There are three configurations that you can implement; one for each support.
Each configuration type will be described below.

Each support will have its own API that matches its functionality.

## Keyboard

The [IKeyboardInputHandler](./controllers/keyboard/IKeyboardInputHandler.cs)
will notify any key input from a standard keyboard. Depending on the
manufacturer, some keys might not be supported as they require special
software/firmware to be detected by the OS.

### Keyboard Configuration

The [IKeyboardInputConfig](./controllers/keyboard/IKeyboardInputConfig.cs)
defines a set of keys to be listen to and a `GetAction` method that converts the
input key into an [InputAction](./InputAction.cs)

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
        KeyList.Space,
        KeyList.Enter
    };

    public InputAction GetAction(KeyList key)
    {
        if (!_acceptedInput.Contains(key))
        {
            return InputAction.None; // Ignore input not in list
        }

        switch (key)
        {
            case KeyList.Up: return InputAction.Up; // Move forward
            case KeyList.Down: return InputAction.Down; // Move backwards
            case KeyList.Space: return InputAction.South; // Jump
            //
            default: return InputAction.Unknown; // KeyList.Enter not handled
        }
    }
}
```

### Keyboard Handler

We can enable the
[IKeyboardInputHandler](./controllers/keyboard/IKeyboardInputHandler.cs) passing
to it a [IKeyboardInputConfig](./controllers/keyboard/IKeyboardInputConfig.cs).
The [IKeyboardInputHandler](./controllers/keyboard/IKeyboardInputHandler.cs) is
a service that needs to be located.

```cs
using Godot;
using Lavos.Input;
using Lavos.Dependency;

public class PlayerController
    : Node
{
    IKeyboardInputHandler _keyboardHandler = null;
    IKeyboardInputConfig _config = new KeyboardConfig();

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

Implement the
[IKeyboardInputListener](./controllers/keyboard/IKeyboardInputListener.cs)
interface register for keyboard events.

```cs
using Godot;
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

    #endregion IKeyboardInputListener

    // ...
}
```

To receive keyboard events, register as a listener to
[IKeyboardInputHandler](./controllers/keyboard/IKeyboardInputHandler.cs)

```cs
using Godot;
using Lavos.Input;
using Lavos.Dependency;

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

The [IMouseInputHandler](./controllers/mouse/IMouseInputHandler.cs) will notify
position changes as well as mouse button events. Depending on the manufacturer,
some keys might not be supported as they require special software/firmware to be
detected by the OS.

### Mouse Configuration

The [IMouseInputConfig](./controllers/mouse/IMouseInputConfig.cs) defines a set
of mouse buttons it will listen to and a `GetAction` method that converts the
input button into an [InputAction](./InputAction.cs)

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
            //
            default: return InputAction.Unknown;
        }
    }
}
```

### Mouse Handler

We can enable the
[IMouseInputHandler](./controllers/mouse/IMouseInputHandler.cs) passing to it a
[IMouseInputConfig](./controllers/mouse/IMouseInputConfig.cs). The
[IMouseInputHandler](./controllers/mouse/IMouseInputHandler.cs) is a service
that needs to be located.

```cs
using Godot;
using Lavos.Input;
using Lavos.Dependency;

public class LazerGun
    : Node
{
    IMouseInputHandler _mouseHandler = null;
    IMouseInputConfig _config = new MouseConfig();

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

Implement the [IMouseInputListener](./controllers/mouse/IMouseInputListener.cs)
interface to register for mouse events

```cs
using Godot;
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

To receive mouse events, register as a listener to
[IMouseInputHandler](./controllers/mouse/IMouseInputHandler.cs)

```cs
using Godot;
using Lavos.Input;
using Lavos.Dependency;

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

The gamepad handler will notify any gamepad input send from a standard
controller.

### Gamepad Configuration

The [IGamepadInputConfig](./controllers/gamepad/IGamepadInputConfig.cs) defines
a set of buttons and axis it will listen to and the following methods that
converts gamepad input into an [InputAction](./InputAction.cs):
`GetActionState`, `GetAxisState` and `GetTriggerState`.

```c#
using Godot;
using Lavos.Input;
using System.Collections.Generic;

public class GamepadConfig
    : IGamepadInputConfig
{
    IReadOnlyCollection<GamepadAxis> IGamepadInputConfig.Axis => _axis;
    readonly HashSet<GamepadAxis> _axis = new HashSet<GamepadAxis>()
    {
        GamepadAxis.LeftStick, GamepadAxis.RightStick,
        GamepadAxis.LeftTrigger, GamepadAxis.RightTrigger
    };

    IReadOnlyCollection<GamepadButtons> IGamepadInputConfig.Buttons => _buttons;
    readonly HashSet<GamepadButtons> _buttons = new HashSet<GamepadButtons>()
    {
        GamepadButtons.South, 
        GamepadButtons.East, 
        GamepadButtons.West, 
        GamepadButtons.North,
    };

    InputAction IGamepadInputConfig.GetActionState(GamepadButtons button)
    {
        if (_buttons.Contains(button) == false)
        {
            return InputAction.None;
        }
        //
        switch (button)
        {
            case GamepadButtons.South: return InputAction.South;
            case GamepadButtons.East: return InputAction.East;
            case GamepadButtons.West: return InputAction.West;
            case GamepadButtons.North: return InputAction.North;
            //
            default: return InputAction.Unkwnon;
        }
    }

    InputAction IGamepadInputConfig.GetAxisState(GamepadAxis axis, float value)
    {
        if (_axis.Contains(axis) == false)
        {
            return InputAction.None;
        }
        //
        switch (axis)
        {
            case GamepadAxis.LeftStick: return InputAction.LeftStick;
            case GamepadAxis.RightStick: return InputAction.RightStick;
            //
            default: return InputAction.Unkwnon;
        }
    }

    InputAction IGamepadInputConfig.GetTriggerState(GamepadAxis trigger, float pressure)
    {
        if (_axis.Contains(trigger) == false)
        {
            return InputAction.None;
        }
        //
        const float THRESHOLD = 0.1f;
        if (pressure < THRESHOLD)
        {
            return InputAction.None;
        }
        //
        switch (trigger)
        {
            case GamepadAxis.LeftTrigger: return InputAction.LeftTrigger;
            case GamepadAxis.RightTrigger: return InputAction.RightTrigger;
            //
            default: return InputAction.Unkwnon;
        }
    }
}
```

### Gamepad Input Handler

We can enable the gamepad handler for a specific gamepad by passing to it a
[GamepadDevice](./controllers/gamepad/Gamepads.cs) and a
[IGamepadInputConfig](./controllers/gamepad/IGamepadInputConfig.cs). The
[IGamepadInputHandler](./controllers/gamepad/IGamepadInputHandler.cs) is a
service that needs to be located.

```c#
using Godot;
using Lavos.Input;
using Lavos.Dependency;

public class PlayerController
    : Node
{
    IGamepadInputHandler _gamepadInputHandler = null;
    IGamepadInputConfig _config = new GamepadConfig();

    public override void _EnterTree()
    {
        _gamepadInputHandler = ServiceLocator.Locate<IGamepadInputHandler>();
        _gamepadInputHandler.EnableHandler(GamepadDevice.Gamepad1, _config);
    }

    public override void _ExitTree()
    {
        _gamepadInputHandler.EnableHandler(GamepadDevice.Gamepad1, null);
    }
}
```

### Gamepad Input Listener

Implement the
[IGamepadInputListener](./controllers/gamepad/IGamepadInputListener.cs)
interface to register for keyboard events

```c#
using Godot;
using Lavos.Input;
using Lavos.Dependency;

public class PlayerController
    : Node
    , IGamepadInputListener
{
    IGamepadInputHandler _gamepadInputHandler = null;
    IGamepadInputConfig _config = new GamepadConfig();

    #region IGamepadInputListener

    GamepadDevice IGamepadInputListener.Gamepad => GamepadDevice.Gamepad1;
    int IGamepadInputListener.Priority => 0;

    bool IGamepadInputListener.OnTriggerValueChanged(GamepadDevice device, InputAction action, float value)
    {
        Log.Debug(Tag, $"Gamepad {device} trigger value changed {action} - {value}");
        return true;
    }

    bool IGamepadInputListener.OnAxisValueChanged(GamepadDevice device, InputAction action, Vector2 value)
    {
        Log.Debug(Tag, $"Gamepad {device} axis value changed {action} - {value}");
        return true;
    }

    bool IGamepadInputListener.OnGamepadButtonPressed(GamepadDevice device, InputAction action)
    {
        Log.Debug(Tag, $"Gamepad {device} Button {action} pressed");
        return true;
    }

    bool IGamepadInputListener.OnGamepadButtonReleased(GamepadDevice device, InputAction action)
    {
        Log.Debug(Tag, $"Gamepad {device} Button {action} released");
        return true;
    }

    #endregion IGamepadInputListener

    //...
}
```

To receive gamepad events, register as a listener to
[IGamepadInputHandler](./controllers/mouse/IMouseInputHandler.cs)

```c#
using Godot;
using Lavos.Input;
using Lavos.Dependency;

public class PlayerController
    : Node
    , IGamepadInputListener
{
    //...

    public override void _EnterTree()
    {
        _gamepadInputHandler = ServiceLocator.Locate<IGamepadInputHandler>();
        //...
    }

    public override void _ExitTree()
    {
        _gamepadInputHandler.UnregisterListener(this);
        //...
    }

    public override void _Ready()
    {
        _gamepadInputHandler.RegisterListener(this);
    }
}
```

### Gamepad Status Changes

To receive notifications whenever the gamepad status changes, register a
[IGamepadStatusListener](./controllers/gamepad/IGamepadStatusListener.cs) with
the [IGamepadStatusHandler](./controllers/gamepad/IGamepadStatusHandler.cs)
service.

```c#
using Godot;
using Lavos.Input;
using Lavos.Dependency;

public class ControllerStatus
    : Node
    , IGamepadStatusListener
{
    IGamepadStatusHandler _GamepadStatusHandler = null;

    #region IGamepadStatusListener

    void IGamepadStatusListener.OnGamepadConnected(GamepadDevice device)
    {
        Log.Debug(Tag, $"Gamepad {device} connected");
    }

    void IGamepadStatusListener.OnGamepadDisconnected(GamepadDevice device)
    {
        Log.Debug(Tag, $"Gamepad {device} disconnected");
    }

    #endregion IGamepadStatusListener

    public override void _EnterTree()
    {
        _GamepadStatusHandler = ServiceLocator.Locate<IGamepadStatusHandler>();
    }

    public override void _ExitTree()
    {
        _GamepadStatusHandler.UnregisterListener(this);
    }

    public override void _Ready()
    {
        _GamepadStatusHandler.RegisterListener(this);
    }
}
```
