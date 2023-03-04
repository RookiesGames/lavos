#!/bin/bash

#
# Homebrew
#
echo "Homebrew"
if ! command -v brew &>/dev/null; then
    echo "\tHomebrew could not be found. Installing it..."
    /bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"
fi

echo "Updating Homebrew..."
brew update
echo "Upgrading formulaes..."
brew upgrade
echo "Done!"
echo ""

#
# V toolchain
#
if ! command -v v &>/dev/null; then
    echo "V could not be found. Installing it..."
    brew install vlang
fi

v up
echo "Done!"
