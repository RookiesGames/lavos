#!/usr/bin/env -S v run

import os

const project_name = "game"

fn main() {
	wd := os.getwd()

	// Build Lavos build tool
	command("v run ${wd}/lavos/tools/build_tool/build.vsh")
	// Run project setup
	project_path := "${wd}/${project_name}"
	command("${wd}/lavos/tools/build_tool/bin/lbt godot clean ${project_path}")
	command("${wd}/lavos/tools/build_tool/bin/lbt godot setup ${project_path}")
	command("${wd}/lavos/tools/build_tool/bin/lbt plugins android build")
}

fn command(cmd string) {
	result := os.execute(cmd)
	println(result.output)
}