#!/usr/bin/env -S v run

import cli
import os

const (
	godot_project            = 'project.godot'
	symlink_rookies          = 'rookies'
	symlink_script_templates = 'script_templates'
)

//////////
// Check

fn cmd_check(cmd cli.Command) ! {
	path := cmd.args[0]
	check_project(path)!
}

fn check_project(path string) ! {
	print('~> Entering project directory ${path}')
	chdir(path) or {
		println(' ❌')
		return err
	}
	println(' ✅')
	//
	print('~> Checking for valid Godot project in ${path}')
	if is_file(godot_project) == false {
		return error('Not a valid Godot project, missing ${godot_project}')
	}
	//
	println(' ✅')
}

fn check_path(path string) ! {
	if !os.is_dir(path) {
		return error('path ${path} does not exist')
	}
}

//////////
// Setup

fn execute_command(msg string, cmd string) ! {
	print(msg)
	flush()
	res := os.execute(cmd)
	if res.exit_code != 0 {
		println(' ❌')
		return error(res.output)
	}
	println(' ✅')
}

fn cmd_setup(cmd cli.Command) ! {
	path := cmd.args[0]
	check_path(path)!
	//
	println('~> Setting up project...')
	execute_command('\t~> Initialise submodules', 'git submodule init')!
	execute_command('\t~> Updating submodules', 'git submodule update --recursive')!
	//
	cmd_clean(cmd)!
	cmd_link(cmd)!
	//
	println('~> Setup up complete!')
}

//////////
// Clean

fn cmd_clean(cmd cli.Command) ! {
	path := cmd.args[0]
	check_path(path)!
	//
	println('~> Cleaning project...')
	remove_symlink('${path}/${symlink_script_templates}')!
	remove_symlink('${path}/addons/${symlink_rookies}')!
	remove_symlink('${path}/android/plugins')!
	// remove_symlink('$path/ios/plugins')!
	println('~> Cleaning complete!')
}

fn remove_symlink(link string) ! {
	if is_file(link) || is_link(link) {
		print('\t~> Removing symlink ${link}')
		rm(link) or {
			println(' ❌')
			return err
		}
		//
		println(' ✅')
	} else if is_dir(link) {
		print('\t~> Removing symlink ${link}')
		rmdir(link) or {
			println(' ❌')
			return err
		}
		//
		println(' ✅')
	}
}

/////////
// Link

fn cmd_link(cmd cli.Command) ! {
	path := cmd.args[0]
	check_path(path)!
	//
	println('~> Creating links...')
	lavoswd := os.getwd()
	// lavos source/addons
	create_path('${path}/addons')!
	create_symlink('${lavoswd}/lavos/addons', '${path}/addons/${symlink_rookies}')!
	// script templates (BROKEN as of Godot 4.1.1)
	// create_symlink('${lavoswd}/lavos/${symlink_script_templates}', '${path}/${symlink_script_templates}')!
	// plugins
	create_path('${path}/android')!
	create_symlink('${lavoswd}/lavos/plugins/android/godot', '${path}/android/plugins')!
	// create_path('$path/ios')!
	// create_symlink('$lavoswd/lavos/plugins/ios/godot', '$path/ios/plugins')!
	//
	println('~> Links created!')
}

fn create_symlink(symlink_source string, symlink_target string) ! {
	print('\t~> Creating ${symlink_target} symlink')
	symlink(symlink_source, symlink_target) or {
		println(' ❌')
		return err
	}
	//
	println(' ✅')
}

fn create_path(path string) ! {
	if os.is_dir(path) {
		return
	}
	//
	print('\t~> Creating folder: ${path}')
	mkdir_all(path) or {
		println(' ❌')
		return err
	}
	println(' ✅')
}

////////
// CLI

fn main() {
	mut app := cli.Command{
		name: 'lavos'
		description: 'Set of tools to setup lavos'
		execute: fn (cmd cli.Command) ! {
			cmd.execute_help()
		}
		commands: [
			cli.Command{
				name: 'check'
				usage: '<target>'
				description: 'Check if path is a valid Godot project'
				execute: fn (cmd cli.Command) ! {
					cmd_check(cmd)!
				}
				required_args: 1
			},
			cli.Command{
				name: 'setup'
				usage: '<target>'
				description: 'Setup a Godot project with Lavos'
				execute: fn (cmd cli.Command) ! {
					cmd_setup(cmd)!
				}
				required_args: 1
			},
			cli.Command{
				name: 'clean'
				usage: '<target>'
				description: 'Clean previously generated symlinks'
				execute: fn (cmd cli.Command) ! {
					cmd_clean(cmd)!
				}
				required_args: 1
			},
			cli.Command{
				name: 'link'
				usage: '<target>'
				description: 'Create symlinks to lavos tools'
				execute: fn (cmd cli.Command) ! {
					cmd_link(cmd)!
				}
				required_args: 1
			},
		]
	}
	app.setup()
	app.parse(os.args)
}
