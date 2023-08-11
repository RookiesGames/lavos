#!/usr/bin/env -S v run

import os
import os.cmdline

const (
	project = cmdline.options(os.args, '-p')[0]
)

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

fn init_modules() ! {
	execute_command('~> Initialise submodules', 'git submodule init')!
}

fn update_modules() ! {
	execute_command('~> Updating submodules', 'git submodule update --recursive')!
}

fn setup_lavos() ! {
	mut wd := os.dir(@FILE)
	$if macos { wd = wd.replace(" ", "\\ ") }
	//
	cmd := $if windows { 'cmd /c v ' } $else { 'v' }
	//
	execute_command('~> Cleaning up previous lavos setup', '${cmd} run ${wd}/lavos.vsh clean \"${project}\"')!
	execute_command('~> Setting up lavos', '${cmd} run ${wd}/lavos.vsh link \"${project}\"')!
}

if project.is_blank() {
	panic('Missing project path -p paramenter')
} else {
	println('Project path: ${project}')
}

init_modules()!
update_modules()!
setup_lavos()!
println('Setup Complete!')
