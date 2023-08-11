#!/bin/bash
cd -- "$(dirname "$BASH_SOURCE")"

# Variables

# Install environment
./lavos/tools/env/installEnv.command

# Setup project
path="$(pwd)/<project_folder>"
v run ./lavos/tools/setupProject.vsh setup "$path"
