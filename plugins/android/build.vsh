#!/usr/bin/env -S v run

import cli
import os

const (
	project_dir   = 'project'
	// Commands
	command_help  = 'help'
	command_build = 'build'
	command_copy  = 'copy'
	command_clean = 'clean'
	// Arguments
	arg_all       = 'all'
	arg_fa        = 'FirebaseAnalytics'
	arg_fc        = 'FirebaseCrashlytics'
	projects      = [arg_fa, arg_fc]
)

type JobFn = fn () !

fn do_job(cb JobFn) {
	wd := getwd()
	defer {
		chdir(wd) or {}
	}
	cb() or { println('Failed to execute job.\n{err}') }
}

fn check_arg(arg string) ! {
	if arg != arg_all && arg !in projects {
		println('Unrecognized argument. Possible values are: {arg_all}, {arg_fa}, {arg_fc}')
		return
	}
}

//

fn cmd_clean(cmd cli.Command) ! {
	job := fn () ! {
		clean_builds()!
	}
	do_job(job)
}

fn clean_builds() ! {
	println('~> Cleaning previous builds')
	//
	wd := os.dir(@FILE)
	chdir('$wd/project')!
	//
	cmd := './gradlew clean'
	print('\t~> Running command {cmd}...')
	mut res := execute(cmd)
	if res.exit_code != 0 {
		println(' ❌')
		return error('$res.output')
	}
	//
	println(' ✅')
}

fn cmd_copy(cmd cli.Command) ! {
	arg := cmd.args[0]
	check_arg(arg)!
	//
	job := fn [arg] () ! {
		copy_builds(arg)!
	}
	do_job(job)
}

fn copy_builds(option string) ! {
	println('~> Copy build output')
	// Copy output to folders
	if option == arg_all {
		for proj in projects {
			copy_output(proj) or {
				println('{err}')
				continue
			}
		}
	} else {
		copy_output(option)!
	}
}

fn copy_output(proj string) ! {
	wd := os.dir(@FILE)
	src := '$wd/project/{proj}/build/outputs/aar/{proj}-release.aar'
	dst := '$wd/godot/{proj}.aar'
	//
	if !is_file(src) {
		println('\t~> File {src} not found')
		return
	}
	//
	print('\t~> Copying output of {proj}')
	cp(src, dst) or {
		println(' ❌')
		return error('Failed to copy from {src} to {dst}')
	}
	println(' ✅')
	println('\t\tfrom: {src}')
	println('\t\tto: {dst}')
	//
	gdap := '$wd/godot/{proj}.gdap'
	print('\t~> Checking for GDAP file in {gdap}')
	if !is_file('{gdap}') {
		println(' ❌')
		return error('Misisng {gdap} file')
	}
	//
	println(' ✅')
}

fn cmd_build(cmd cli.Command) ! {
	arg := cmd.args[0]
	check_arg(arg)!
	//
	build_job := fn [arg] () ! {
		start_builds(arg)!
	}
	do_job(build_job)
}

fn start_builds(option string) ! {
	println('~> Starting build jobs')
	//
	wd := os.dir(@FILE)
	chdir('$wd/project')!
	//
	mut failure := false
	if option == arg_all {
		for proj in projects {
			build_project(proj) or { failure = true }
		}
	} else {
		if option in projects {
			build_project(option) or { failure = true }
		} else {
			failure = true
			println('Project {option} not recognized')
		}
	}
	//
	if failure {
		return error('Error when building projects. Aborting...')
	}
	//
	println('~> Builds completed!')
}

fn build_project(proj string) ! {
	cmd := './gradlew build :{proj}:assembleRelease'
	print('\t~> Running command {cmd}...')
	res := execute(cmd)
	if res.exit_code != 0 {
		println(' ❌')
		return error('{res.output}')
	}
	//
	println(' ✅')
}

fn main() {
	mut app := cli.Command{
		name: 'build.vsh'
		description: 'Build LavosPlugins for Android'
		execute: fn (cmd cli.Command) ! {
			cmd.execute_help()
		}
		commands: [
			cli.Command{
				name: 'clean'
				description: 'Clean previously generated Android builds'
				execute: fn (cmd cli.Command) ! {
					cmd_clean(cmd)!
				}
			},
			cli.Command{
				name: 'copy'
				usage: '<target>'
				description: 'Copy generated Android package into the output folder'
				execute: fn (cmd cli.Command) ! {
					cmd_copy(cmd)!
				}
				required_args: 1
			},
			cli.Command{
				name: 'build'
				usage: '<target>'
				description: 'Build target Android package'
				execute: fn (cmd cli.Command) ! {
					cmd_build(cmd)!
				}
				required_args: 1
			},
		]
	}
	app.setup()
	app.parse(os.args)
}
