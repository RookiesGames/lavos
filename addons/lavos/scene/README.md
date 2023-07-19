# Scene

Scene module

## Index

* [Bootstrap](#bootstrap)
    * [Omni Node](#omni-node)
* [Node Tree](#node-tree)
* [Scene Manager](#scene-manager)

## Bootstrap

To optimally use Lavos, it is recommended to start your boot process using an [OmniNode](./OmniNode.cs).

### Omni Node

The [OmniNode](./OmniNode.cs) should be your starting point for your game.

In your main scene create a single `Node` and attach the [OmniNode](./OmniNode.cs) script to it.
The `OmniNode` node exposes two properties:
* The following scene
* A list of configurations. Read mode about it [here](../dependency/README.md)

Lavos comes with its own configuration resource that you should add to the list as a first requirement. It is then suggested that you create your own [ConfigList](../dependency/ConfigList.cs) with game specific [Config](../dependency/README.md)s.

## Node Tree

The [NodeTree](./NodeTree.cs) will automatically be available to you if you
use the bootstrap process described [above](#bootstrap).

The `NodeTree` is a convinient node that facilitates retrieving nodes
either by name or by type. The only limitation is for a key to be unique,
that is, no two names or two types can be registered at the same time.

```c#
using Godot;
using Lavos.Scene;

public sealed class Player : Node
{
    public override void _Ready()
    {
        NodeTree.PinNode<Player>(this);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNode<Player>();
    }
}

public sealed class Enemy
{
    public override void _Ready()
    {
        var player = NodeTree.GetPinnedNode<Player>();
        var position = player.Position;
        //...
    }
}

public sealed class ButtonFactory : Control
{
    public const string PlayerNameKey = "PlayerName";
    public const string PlayerTeamNameKey = "PlayerTeamName";

    Label _playerName;
    Label _playerTeamName;

    public override void _Ready()
    {
        NodeTree.PinNode(PlayerNameKey, _playerName);
        NodeTree.PinNode(PlayerTeamNameKey, _playerTeamName);
    }

    public override void _ExitTree()
    {
        NodeTree.UnpinNode(PlayerNameKey);
        NodeTree.UnpinNode(PlayerTeamNameKey);
    }
}
```

## Scene Manager

The [SceneManager](./SceneManager.cs) will automatically be available to you if you
use the bootstrap process described [above](#bootstrap).

It provides an easy way to change between scenes or to instanciate a scene into the current one.
