#!/bin/bash
cd -- "$(dirname "$BASH_SOURCE")"

# Setup project
v run ./lavos/tools/templates/setupProject.vsh
