# Dependency

Dependency module

## Index

- [Services](#services)
  - [Registering a Service](#registering-a-service)
    - [Config](#config)
      - [Configure](#configure)
      - [Initialize](#initialize)
    - [ServiceLocator](#service-locator)
      - [Register](#register)
      - [Locate](#locate)

## Services

Services are entities that operate on a very specific area. They inherit from
Node but it is not mandatory.

To define a service, inherit from [IService](./IService.cs). The dependency
manager can only work with `IService`s.

### Registering a Service

There are two ways to register a service

- Using [Config](./Config.cs)
- Using [ServiceLocator](./ServiceLocator.cs)

#### Config

A [Config](./Config.cs) is a resource file that can be created and added to the
initial [bootstrap process](../scene/README.md).

Create a new resource and assign to it the [Config](./Config.cs) script. Configs
define two methods: `Configure` and `Initialize`

##### Configure

Use this step to bind services

```c#
public sealed class InputConfig : Config
{
    public override void Configure(IDependencyBinder binder)
    {
#if CONSOLE
        // Only Gamepad support
        binder.Bind<IGamepadInputHandler, GamepadInputHandler>();
        binder.Bind<IGamepadStatusHandler, GamepadStatusHandler>();

        // Mock other input types
        binder.Bind<IKeyboardInputHandler, DummyKeyboardInputHandler>();
        binder.Bind<IMouseInputHandler, DummyMouseInputHandler>();
#elif MOBILE
        // Mock all input
        binder.Bind<IKeyboardInputHandler, DummyKeyboardInputHandler>();
        binder.Bind<IMouseInputHandler, DummyMouseInputHandler>();
        binder.Bind<IGamepadInputHandler, DummyGamepadInputHandler>();
        binder.Bind<IGamepadStatusHandler, DummyGamepadStatusHandler>();
#else // DESKTOP
        binder.Bind<IKeyboardInputHandler, KeyboardInputHandler>();
        binder.Bind<IMouseInputHandler, MouseInputHandler>();

        binder.Bind<IGamepadInputHandler, GamepadInputHandler>();
        binder.Bind<IGamepadStatusHandler, GamepadStatusHandler>();
#endif
    }

    //...
}
```

##### Initialize

Use the initialize step to setup the services that need and early
initialization. Don't expect other services to be available at this stage, the
order will depend on the config position in the [ConfigList](./ConfigList.cs)

```c#
public sealed class InputConfig : Config
{
    //...

    public override void Initialize(IDependencyResolver resolver)
    {
        resolver.Resolve<IKeyboardInputHandler>();
        resolver.Resolve<IMouseInputHandler>();
        resolver.Resolve<IGamepadInputHandler>();
        resolver.Resolve<IGamepadStatusHandler>();
    }
}
```

#### Service Locator

The [ServiceLocator](./ServiceLocator.cs) is used to locate services as well as
registering new ones. It is adviced to register services during the
[Config](#config) process but it is not always possible or convinient to do so.

##### Register

When you create a service, you can register it using the `Register` method. Only
interfaces that inherit from [IService](#services) can be located.

```c#
using Lavos.Dependency;

public interface IAudioManager : IService
{
    //...
}

public sealed class AudioManager : IAudioManager
{
    public override void _Ready()
    {
        var manager = ServiceLocator.Register<IAudioManager, AudioManager>(this);
        // IAudioManager now available to be located
    }
}
```

##### Locate

You can locate a service using the `Locate` method. Only interfaces that inherit
from [IService](#services) can be located.

```c#
using Lavos.Dependency;

public sealed class Player
{
    public override void _Ready()
    {
        var manager = ServiceLocator.Locate<IAudioManager>();
        manager.PlaySound();
    }
}
```
