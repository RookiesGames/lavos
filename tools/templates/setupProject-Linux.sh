#!/bin/bash

# Setup project
path="$(pwd)/<project_folder>"
v run ./lavos/tools/lavos.vsh -p $path
