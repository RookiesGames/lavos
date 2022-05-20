# lavos
Godot shared code

# Index

1. [Requirements](#requirements)
1. [Installation](#install)

---

# Requirements

* Godot 3.4.4 or earlier
* V compiler, [link](vlang.io)

# Install

Follow these instructions to install C# support and necessary dependencies

## Fedora

dnf install dotnet-sdk-3.1

## VSCode

First, the C# project needs to be created.
Navigate in the top bar to `Project > Tools > C#` aand select `Create Project`.

VSCode has a `C# Tools for Godot` extensions that adds a fair number of improvements. Install it directly from the `Extensions` tab.

There are some .NET dependencies that are needed for the editor to work properly.
Run these commands in the root of your Godot project: 
* `dotnet add package Godot.NET.Sdk --version 3.3.0`
* `dotnet add package Newtonsoft.Json --version 13.0.1`
