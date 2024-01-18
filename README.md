# Lavos Framework

Lavos is a set of shared functionality that aims to accelerate the development of games using the Godot Engine.
It provides reusable components common to most games, in a `develop once use everywhere` mindset.

---

## Index

1. [Requirements](#requirements)
    * [Godot Version](#godot-version)
1. [Install](#install)
    * [Add submodule](#add-submodule)
    * [Link Lavos](#link-lavos)
1. [Lavos](#lavos)
    * [Source](#source)
    * [Addons](#addons)
    * [Plugins](#plugins)
        * [Android Plugins](#android-plugins)
        * [iOS Plugins](#ios-plugins)
1. [Contact](#contact)
1. [License](#license)

## Requirements

### Godot Version

Lavos is currently targetting Godot 4.x. 
Newer versions might work out of the box, with no guarantee.
Earlier major versions are not compatible anylonger due to the many breaking changes the 4 series introduced.

## Install

At the moment, the only way to start making use of it is by having Lavos as a submodule of your project.
In the future we might look into packaging Lavos into a NuGet package.

### Add submodule

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

### Link Lavos

Instead of copying the `Lavos` into your project each time there is an update, configuration scripts
are provided to keep `Lavos` up to date in your project. Each time you update `Lavos` git module, 
your project will automatically receive those changes.

In case the symbolic links are broken or new ones were added to `Lavos`, you can run the configuration scripts to refresh the setup.

You can find under `lavos/tools/templates` a script of each major OS. Copy the one(s)
you need into the root of your repo and update its content to match your project
folder name

```
<path>/MyGame/
    -> project.godot
    -> icon.png
    -> etc...
<path>/lavos/
<path>/setupScript.{sh|command|bat}
```

## Lavos

`Lavos` provides many features that will make developing games an easier and faster process.
It provides:
- Common, utility and extension code
- Useful addons for selective features
- Plugins to add features to your game

### Source

All the reusable code to be used in a project will live under `addons/rookies/`.
**Being an addons, don't forget to enable it in your project settings!**

Read the [README.md](./addons/lavos/README.md) to learn more about it!

### Addons

Lavos offers a selection of addons that you can use to enhance the features of 
your game. You will find them under `addons/`, next to lavos itself.

### Plugins

Read the [README.md](./plugins/README.md) to learn more aboout it!

## Contact

Feel free to reach out for any query, suggestion, etc.

## License

This project is licensed under the **MIT License**. Enjoy!