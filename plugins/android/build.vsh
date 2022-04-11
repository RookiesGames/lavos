#!/usr/bin/env -S v run

import os
import flag

mut fp := flag.new_flag_parser(os.args)
fp.application('lavosplugins_android_build')
fp.version('v1.0.0')
fp.description('Build LavosPlugins for Android')
fp.skip_executable()
default_opt := 'all'
build_opt := fp.string('build', `b`, default_opt, 'Project to build')

// Start

wd := getwd()

// Project to build
mut projects := []string{}
// Add the plugin folder HERE
projects << 'FirebaseCrashlytics'
projects << 'FirebaseAnalytics'

// Change directory to Android project root
dir := 'project'
println('~> Changing directory to ./$dir')
chdir(dir) or { panic('Failed to change to android project directory\n$err') }

//
println('Starting build...')

// Clean old builds
clean_cmd := './gradlew clean'
print('~> $clean_cmd')
mut res := execute(clean_cmd)
if res.exit_code != 0 {
	println(' ❌')
	println('Failed to clean android project\n$res.output')
	return
} else {
	println(' ✅')
}

// Build all modules
build_cmd := './gradlew build'
mut error := false

if build_opt == default_opt {
	for proj in projects {
		cmd := '$build_cmd :$proj:assembleRelease'
		print('~> $cmd')
		//
		res = execute(cmd)
		if res.exit_code != 0 {
			println(' ❌')
			println('Failed to build module $proj\n$res.output')
			error = true
		}
		else {
			println(' ✅')
		}
	}
}
else {
	if build_opt in projects {
		proj := build_opt
		cmd := '$build_cmd :$proj:assembleRelease'
		print('~> $cmd')
		//
		res = execute(cmd)
		if res.exit_code != 0 {
			println(' ❌')
			println('Failed to build module $proj\n$res.output')
			error = true
		}
		else {
			println(' ✅')
		}
	}
	else {
		error = true
		println('Project $build_opt not recognized')
	}
}

if error {
	println('Error when building projects')
	println('Aborting...')
	return
}

//
println('Build complete!')
println('---')
println('Preparing to copy outputs...')

// Move back to root
chdir(wd) or { panic('Failed to change directory $wd') }

// Copy output to folders
if build_opt == default_opt {
	for proj in projects {
		src := 'project/$proj/build/outputs/aar/$proj-release.aar'
		dst := 'godot/${proj}.aar'
		println('~> Copying output of $proj')
		cp(src, dst) or { panic('Failed to copy from $src to $dst') }
		println('\t✅ $src >> $dst')
		//
		if is_file('godot/${proj}.gdap') {
			print('\t✅ Found')
		} else {
			print('\t❌ Missing')
		}
		println(' godot/${proj}.gdap')
	}
}
else {
	proj := build_opt
	src := 'project/$proj/build/outputs/aar/$proj-release.aar'
	dst := 'godot/${proj}.aar'
	println('~> Copying output of $proj')
	cp(src, dst) or { panic('Failed to copy from $src to $dst') }
	println('\t✅ $src >> $dst')
	//
	if is_file('godot/${proj}.gdap') {
		print('\t✅ Found')
	} else {
		print('\t❌ Missing')
	}
	println(' godot/${proj}.gdap')
}