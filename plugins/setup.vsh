#!/usr/bin/env -S v run

import os
import flag

mut fp := flag.new_flag_parser(os.args)
fp.application('lavos_setup')
fp.version('v1.0.0')
fp.description('Setup a godot project with Lavos')
fp.skip_executable()
path := fp.string('path', `p`, '', 'Location of the Godot project to setup')

if path == '' {
	println('Please enter a path with the --path=<path> option')
	return
}

if is_dir(path) == false {
	println('Path $path is not a directory')
	return
}

lavoswd := getwd()

println('~> Entering directory $path')
chdir(path) or {
	println('Failed to change working directory to $path\n$err')
	return
}

print('~> Checking for valid Godot project in $path')
if is_file('project.godot') == false {
	println(' ❌')
	println('Not a valid Godot project, missing project.godot')
	return
}
println(' ✅')

//
println('~> Creating plugin symlinks...')

//
android_path := '$path/android'
mkdir_all(android_path) or {
	println('\t~> Failed to create Android plugin path\n$err')
	return
}

//
println('\t~> Entering directory $android_path')
chdir(android_path) or {
	println('Failed to change working directoy to $android_path\n$err')
	return
}

//
print('\t~> Creating symlink to Lavos Androd plugins')
if is_dir('plugins') == false {
	res := execute('ln -s $lavoswd/android/godot plugins')
	if res.exit_code != 0 {
		println(' ❌')
		println('\tFailed to create symlink\n$res.output')
		return
	}
	println(' ✅')
}
else {
	println(' ❌')
	println('\tFailed to create symlink, plugins directory already exists')
}

//
println('Setup complete!')
