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
print('~> Creating symlink to Lavos source ')
if is_dir('lavos') == false {
	res := execute('ln -s $lavoswd/godot lavos')
	if res.exit_code != 0 {
		println(' ❌')
		println('\t~> Failed to create symlink\n$res.output')
		return
	}
}
println(' ✅')

//
print('~> Creating symlink to Lavos addons ')
if is_dir('addons') == false {
	mkdir('addons') or {
		println('Failed to change working directory to $path\n$err')
		return
	}
}

res := execute('ln -s $lavoswd/addons addons/lavos')
if res.exit_code != 0 {
	println(' ❌')
	println('\t~> Failed to create symlink\n$res.output')
	return
}
println(' ✅')

//
println('Setup complete!')
