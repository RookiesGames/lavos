#!/usr/bin/env -S v run

import os
import flag

const (
	godot_project    = 'project.godot'
	lavos            = 'lavos'
	addons           = 'addons'
	script_templates = 'script_templates'
)

fn check_args(path string) ! {
	print('~> Checking arguments')
	if path == '' {
		println(' ❌')
		return error('Please enter a path with the --path=<path> option')
	}
	//
	if is_dir(path) == false {
		println(' ❌')
		return error('Path {path} is not a directory')
	}
	//
	println(' ✅')
}

fn check_project(path string) ! {
	print('~> Entering project directory {path}')
	chdir(path) or {
		println(' ❌')
		return err
	}
	println(' ✅')
	//
	print('\t ~> Checking for valid Godot project in {path}')
	if is_file(godot_project) == false {
		return error('Not a valid Godot project, missing {godot_project}')
	}
	//
	println(' ✅')
}

fn create_lavos(lavoswd string) ! {
	println('~> Setting up Lavos source')
	// Remove old symlink
	if is_dir(lavos) {
		print('\t~> Removing previous symlink')
		rm(lavos) or {
			println(' ❌')
			return err
		}
		//
		println(' ✅')
	}
	// Create symlink
	print('\t ~> Creating symlink to Lavos source')
	symlink('$lavoswd/godot', lavos) or {
		println(' ❌')
		return err
	}
	//
	println(' ✅')
}

fn create_addons(lavoswd string) ! {
	println('~> Setting up Lavos addons')
	// Create folder
	if is_dir('addons') == false {
		print('\t ~> Creating addons folder')
		mkdir('addons') or {
			println(' ❌')
			return err
		}
		//
		println(' ✅')
	}
	//
	path := getwd()
	chdir(addons) or { return err }
	// Remove old symlink if exists
	if is_dir(lavos) {
		print('\t~> Removing previous symlink')
		rm(lavos) or {
			println(' ❌')
			return err
		}
		//
		println(' ✅')
	}
	// Create symlink
	print('\t~> Creating symlink')
	symlink('{lavoswd}/{addons}', lavos) or {
		println(' ❌')
		return err
	}
	//
	println(' ✅')
	// Go back
	chdir(path) or { return err }
}

fn create_templates(lavoswd string) ! {
	println('~> Setting up script templates')
	// Remove old symlink if exists
	if is_dir(script_templates) {
		print('\t~> Removing previous symlink')
		rm(script_templates) or {
			println(' ❌')
			return err
		}
		//
		println(' ✅')
	}
	// Create symlink
	print('\t~> Creating symlink')
	symlink('{lavoswd}/{script_templates}', script_templates) or {
		println(' ❌')
		return err
	}
	//
	println(' ✅')
}

//
mut fp := flag.new_flag_parser(os.args)
fp.application('lavos_setup')
fp.version('v1.0.0')
fp.description('Setup a godot project with Lavos')
fp.skip_executable()
path := fp.string('path', `p`, '', 'Location of the Godot project to setup')
lavoswd := getwd()

//
check_args(path)!
check_project(path)!
create_lavos(lavoswd)!
create_addons(lavoswd)!
create_templates(lavoswd)!
println('Setup complete!')
