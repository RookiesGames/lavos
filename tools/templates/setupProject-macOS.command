#!/bin/bash
cd -- "$(dirname "$BASH_SOURCE")"

# Variables

# Install environment
./lavos/tools/installEnv.command

# Setup project
path="$(pwd)/<project_folder>"
v run ./lavos/tools/setupProject.vsh -p $path
