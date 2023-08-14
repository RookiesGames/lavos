#!/bin/bash

# Variables

# Install environment
sh ./lavos/tools/env/installEnv.sh

# Setup project
path="$(pwd)/<project_folder>"
v run ./lavos/tools/lavos.vsh -p $path
