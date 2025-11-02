module main

import cli
import os
import godot
import helper
import plugins

fn main() {
	mut app := cli.Command{
		name:        'lavos builder'
		description: 'Set of tools to setup lavos'
		execute:     fn (cmd cli.Command) ! {
			cmd.execute_help()
		}
		commands:    [
			cli.Command{
				name:        'godot'
				description: 'Setup a Godot project with Lavos'
				execute:     fn (cmd cli.Command) ! {
					cmd.execute_help()
				}
				commands:    [
					cli.Command{
						name:          'setup'
						description:   'Setup a Godot project with Lavos'
						execute:       fn (cmd cli.Command) ! {
							path := cmd.args[0]
							godot.setup(path)!
						}
						required_args: 1
						pre_execute:   fn (cmd cli.Command) ! {
							helper.validate_path(cmd.args[0])!
						}
					},
					cli.Command{
						name:          'clean'
						description:   'Clean Godot project from Lavos'
						execute:       fn (cmd cli.Command) ! {
							path := cmd.args[0]
							godot.clean(path)!
						}
						required_args: 1
						pre_execute:   fn (cmd cli.Command) ! {
							helper.validate_path(cmd.args[0])!
						}
					},
				]
			},
			cli.Command{
				name:     'plugins'
				commands: [
					cli.Command{
						name:        'android'
						description: 'Manage Android binaries'
						execute:     fn (cmd cli.Command) ! {
							cmd.execute_help()
						}
						commands:    [
							cli.Command{
								name:        'build'
								description: 'Build binaries'
								execute:     fn (cmd cli.Command) ! {
									plugins.Android.build()!
								}
							},
							cli.Command{
								name:        'clean'
								description: 'Clean binaries'
								execute:     fn (cmd cli.Command) ! {
									plugins.Android.clean()!
								}
							},
						]
					},
					cli.Command{
						name:        'ios'
						description: 'Manage iOS binaries'
						execute:     fn (cmd cli.Command) ! {
							cmd.execute_help()
						}
						commands:    [
							cli.Command{
								name:        'build'
								description: 'Build binaries'
								execute:     fn (cmd cli.Command) ! {
									plugins.IOS.build()!
								}
							},
							cli.Command{
								name:        'clean'
								description: 'Clean binaries'
								execute:     fn (cmd cli.Command) ! {
									plugins.IOS.clean()!
								}
							},
						]
					},
				]
				execute:  fn (cmd cli.Command) ! {
					cmd.execute_help()
				}
			},
		]
	}

	app.setup()
	app.parse(os.args)
}
