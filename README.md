# lavos
Godot shared code

# Index

1. [Boot](#boot)

# Boot

The starting point of the game should be the `boot` scene.
Add a single `Node` and attach the `OmniNode` script to it. The `OmniNode` component will be our root node where everything will be attached to.

`OmniNode` has a couple of properties to be filled in:
1. Next scene - The scene to load after the configurations have been applied
2. List of configs - a list of `Config`s resources that will be loaded at boot time

[OmniNode documentation](godot/nodes/)
[Config documentation](godot/dependency/)
