#!/bin/bash
cd -- "$(dirname "$BASH_SOURCE")"

# Setup project
path="$(pwd)/<project_folder>"
v run ./lavos/tools/lavos.vsh setup "$path"
