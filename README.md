# Lavos

Lavos is a set of shared functionality that aims to accelerate the development of games using the Godot Engine.
It provides reusable components common to most games, in a `develop once use everywhere` mindset.

---

## Index

1. [Requirements](#requirements)
    * [Godot 4.0](#godot-40)
1. [Install](#install)
    * [Submodule](#submodule)
        * [Add submodule](#add-submodule)
        * [Link Lavos](#link-lavos)
            * [Linux](#linux)
            * [macOS](#macos)
            * [Windows](#windows)
    * [NuGet](#nuget)
1. [Lavos](#lavos)
    * [Source](#source)
    * [Addons](#addons)
    * [Plugins](#plugins)
        * [Android Plugins](#android-plugins)
        * [iOS Plugins](#ios-plugins)
1. [Contact](#contact)
1. [License](#license)

## Requirements

### Godot 4.0

Lavos is currently targetting Godot 4.0.x. 
Newer versions might work out of the box, with no guarantee.
Earlier major versions are not compatible anylonger due to the many breaking changes the 4 series introduced.

## Install

At the moment, the only way to start making use of it is by having Lavos as a submodule of your project.
In the future we might look into packaging Lavos into a NuGet package.

### Submodule

#### Add submodule

Imagine you have the following project structure:

```
<path>/MyGame/
    -> project.godot
    -> icon.png
    -> etc...
```

You can add `Lavos` as a submodule next to your root project folder `MyGame`:

```bash
$ cd <path>
$ git submodule add https://github.com/RookiesGames/lavos.git
```

#### Link Lavos

Instead of copying the `Lavos` into your project each time there is an update, configuration scripts
are provided to keep `Lavos` up to date in your project. Each time you update `Lavos` git module, 
your project will automatically receive those changes.

In case the symbolic links are broken or new ones were added to `Lavos`, you can run the configuration scripts to refresh the setup.

Instead of typing commands each time you need them, I suggest creating a single script file that holds all 
the commands we need to link `Lavos` to your project. Place it next to your Godot project root:

```
<path>/MyGame/
    -> project.godot
    -> icon.png
    -> etc...
<path>/lavos/
<path>/setupScript.{sh|command|bat}
```

##### Linux

```bash
#!/bin/bash

sh ./lavos/tools/installEnv.sh

path="$(pwd)/MyGame"
v run  ./lavos/tools/setupProject/vsh -p $path
```

##### macOS

```bash
!/bin/bash

cd -- "$(dirname "$BASH_SOURCE")"

./lavos/tools/installEnv.command

path="$(pwd)/MyGame"
v run ./lavos/tools/setupProject.vsh -p $path
```

##### Windows

```bash
TODO
```

### NuGet

TODO

## Lavos

`Lavos` provides many features that will make developing games an easier and faster process.
It provides:
- Common, utility and extension code
- Useful addons for selective features
- Plugins to add the features to your game

### Source

All the reusable code to be used in a project will live under `godot/`.
As opposed to addons who can be enabled per project, the shared code will be available
out of the box for all projects.

Read the [README.md](./godot/README.md) to learn more aboout it!

### Addons

### Plugins

#### Android Plugins

Read the [README.md](./plugins/README.md) to learn more aboout it!

#### iOS Plugins

## Contact

## License
