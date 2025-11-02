#!/usr/bin/env -S v run

import cli
import os

const project_dir = 'project'

// Commands
const command_help = 'help'
const command_build = 'build'
const command_copy = 'copy'
const command_clean = 'clean'

// Arguments
const arg_firebase_analytics = 'FirebaseAnalytics'
const arg_firebase_crashlytics = 'FirebaseCrashlytics'
const arg_google_billing = 'GoogleBilling'
const arg_google_admob = 'AdMob'
const arg_google_playgames = 'GooglePlayGames'
const arg_ironsource = 'ironSource'
const arg_deviceinfo = 'DeviceInfo'
const projects = get_projects()

type JobFn = fn () !

fn get_projects() []string {
	mut a := []string{}
	a << arg_firebase_analytics
	a << arg_firebase_crashlytics
	a << arg_google_billing
	a << arg_google_admob
	a << arg_google_playgames
	a << arg_ironsource
	return a
}

fn do_job(cb JobFn) {
	wd := getwd()
	defer {
		chdir(wd) or {}
	}
	cb() or { println('Failed to execute job.\n${err}') }
}

fn check_arg(cmd cli.Command) !string {
	arg := if cmd.args.len > 0 { cmd.args[0] } else { '' }
	if !arg.is_blank() && arg !in projects {
		error('Unrecognized argument. Possible values are: ${projects}')
	}
	//
	return arg
}


fn cmd_build(cmd cli.Command) ! {
	arg := check_arg(cmd)!
	build_job := fn [arg] () ! {
		start_builds(arg)!
	}
	do_job(build_job)
}

fn start_builds(option string) ! {
	println('~> Starting build jobs')
	//
	wd := os.dir(@FILE)
	chdir('${wd}/project')!
	//
	mut failure := false
	if option.is_blank() {
		for proj in projects {
			build_project(proj) or { failure = true }
		}
	} else {
		if option in projects {
			build_project(option) or { failure = true }
		} else {
			failure = true
			println('Project ${option} not recognized')
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
	job := fn (cmd string) ! {
		print('\t~> Running command ${cmd}...')
		res := execute(cmd)
		if res.exit_code != 0 {
			println(' ❌')
			return error('${res.output}')
		}

		//
		println(' ✅')
	}
	//
	job('./gradlew build :${proj}:assembleDebug')!
	job('./gradlew build :${proj}:assembleRelease')!
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
				name: 'build'
				usage: '<target>'
				description: 'Build target Android package. Possible values: ${projects}'
				execute: fn (cmd cli.Command) ! {
					cmd_build(cmd)!
				}
			},
		]
	}
	app.setup()
	app.parse(os.args)
}
