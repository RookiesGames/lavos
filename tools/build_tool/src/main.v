module main

import cli
import os
import actions
import helper

fn main() {
	mut app := cli.Command{
		name:        'lavos builder'
		description: 'Set of tools to setup lavos'
		execute:     fn (cmd cli.Command) ! {
			cmd.execute_help()
		}
		commands:    [
			cli.Command{
				name:          'setup'
				usage:         '<target>'
				description:   'Setup a Godot project with Lavos'
				execute:       fn (cmd cli.Command) ! {
					path := helper.validate_path(cmd.args[0])!
					actions.setup(path)!
				}
				required_args: 1
			},
			cli.Command{
				name:          'clean'
				usage:         '<target>'
				description:   'Clean previous setup'
				execute:       fn (cmd cli.Command) ! {
					path := helper.validate_path(cmd.args[0])!
					actions.clean(path)!
				}
				required_args: 1
			},
		]
	}

	app.setup()
	app.parse(os.args)
}
