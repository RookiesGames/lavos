module actions

import os

const godot_project = 'project.godot'

fn check(path string) ! {
	check_path(path)!
	check_project(path)!
}

fn check_path(path string) ! {
	print('Entering project directory ${path}')
	//
	os.chdir(path) or {
		println(' error')
		return err
	}
	//
	println(' ok')
}

fn check_project(path string) ! {
	print('Checking for valid Godt project in ${path}')
	if !os.is_file(godot_project) {
		return error('Not a valid Godot project, missing ${godot_project}')
	}
	//
	println(' ok')
}
