#!/bin/bash

# Variables

# Install environment
sh ./lavos/tools/installEnv.sh

# Setup project
path="$(pwd)/DreamDevour"
v run ./lavos/tools/setupProject.vsh -p $path
