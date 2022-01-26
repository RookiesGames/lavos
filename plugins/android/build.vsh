#!/usr/bin/env -S v run

wd := getwd()

// Project to build
mut projects := []string{}
// Add the plugin folder HERE
projects << 'LavosTest'

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
	panic('Failed to clean android project\n$res.output')
} else {
	println(' ✅')
}

// Build all modules
build_cmd := './gradlew build'
for proj in projects {
	cmd := '$build_cmd :$proj:assembleRelease'
	print('~> $cmd')
	//
	res = execute(cmd)
	if res.exit_code != 0 {
		println(' ❌')
		println('Failed to build module $proj\n$res.output')
	}
	println(' ✅')
}

//
println('Build complete!')
println('---')
println('Preparing to copy outputs...')

// Move back to root
chdir(wd) or { panic('Failed to change directory $wd') }

// Copy output to folders
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
