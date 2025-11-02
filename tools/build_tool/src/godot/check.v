module godot

import os

const godot_project = 'project.godot'

fn check(path string) ! {
	check_project(path)!
}

fn check_project(path string) ! {
	print('Checking for valid Godot project in ${path}')
	if !os.is_file('${path}/${godot_project}') {
		return error('Not a valid Godot project, missing ${godot_project}')
	}
	//
	println(' ok')
}
