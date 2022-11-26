#!/usr/bin/env -S v run

import cli
import os
import strconv

const (
	min_major   = 0
	min_minor   = 3
	min_patch   = 2
	min_version = '${min_major}.${min_minor}.${min_patch}'
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

fn cmd_check() ! {
	println('~> Checking system...')
	check_linux()!
	check_apple()!
	check_windows()!
	check_git()!
	check_v()!
	check_vversion()!
	println('~> Check complete!')
}

fn check_git() ! {
	execute_command('\t~> Is Git installed?', 'git version')!
}

fn check_linux() ! {
	$if linux {
	}
}

fn check_apple() ! {
	$if darwin {
		check_homebrew()!
	}
}

fn check_windows() ! {
	$if windows {
	}
}

fn check_homebrew() ! {
	execute_command('\t~> Is Homebrew installed?', 'brew config')!
}

fn check_v() ! {
	execute_command('\t~> Is V installed?', 'v version')!
}

fn check_vversion() ! {
	print('\t~> Expecting V version >= ${min_version}')
	res := os.execute('v version')
	split := res.output.split(' ')
	// split[0] = V
	// split[1] = major.minor.patch
	// split[2] = commit hash
	version := split[1].split('.')
	major := strconv.atoi(version[0])!
	minor := strconv.atoi(version[1])!
	patch := strconv.atoi(version[2])!
	print(', found ${major}.${minor}.${patch}')
	//
	mut ok := (major > min_major)
	ok = ok || (major == min_major && minor > min_minor)
	ok = ok || (major == min_major && minor == min_minor && patch >= min_patch)
	if ok {
		println(' ✅')
		return
	}
	//
	println(' ❌')
	return error('Outdated version. Expected V ${min_version}, found V ${major}.${minor}.${patch}')
}

fn cmd_install() ! {
	println('~> Installing missing tools...')
	install_linux() !
	install_apple()!
	install_windows() !
	install_git()!
	install_v()!
	println('~> Installation complete!')
}

fn install_linux() ! {
	check_linux() or {}
}

fn install_apple() ! {
	check_homebrew() or {
		msg := '\t~> Installing Homebrew'
		cmd := '/bin/bash -c \"$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)\"'
		execute_command(msg, cmd) !
	}
}

fn install_windows() ! {
	check_windows() or {}
}

fn install_git() ! {
	check_git() or {
		install_fn := fn (cmd string) ! {
			execute_command('\t~> Installing Git', cmd)!
		}

		$if linux {
			// RPM-based
			mut res := os.execute('dnf --version')
			if res.exit_code == 0 {
				install_fn('sudo dnf install git')!
				return
			}

			// Debian-based
			res = os.execute('apt-get --version')
			if res.exit_code == 0 {
				install_fn('sudo apt-get install git')!
				return
			}

			// Arch-based
			res = os.execute('pacman --version')
			if res.exit_code == 0 {
				install_fn('sudo pacman -S git')!
				return
			}
		} $else $if darwin {
			install_fn('brew install git')!
		} $else $if windows {
			return error('Not implemented')
		}
	}
}

fn install_v() ! {
	check_v() or {
		// do install
		$if linux || darwin {
			v_path := '${os.home_dir()}/v'

			// Remove any old artifct
			if os.is_dir(v_path) {
				print('\t~> Removing old V install')
				os.rmdir_all(v_path) or {
					println(' ❌')
					return error('Failed to remove ${v_path}')
				}
				println(' ✅')
			}

			// Clone repo
			execute_command('\t~> Cloning V', 'git clone https://github.com/vlang/v ${v_path}')!

			// Enter folder
			os.chdir(v_path)!

			// Make V
			execute_command('\t~> Making V', 'make')!

			// Create symlink
			execute_command('\t~> Creating symlink to V binary', 'sudo ./v symlink')!
		}
	}
	//
	check_vversion() or {
		// Update V
		execute_command('\t~> Updating V', 'v up')!
	}
}

fn main() {
	mut app := cli.Command{
		name: 'lavosEnv'
		description: 'Set up environment for using lavos tools'
		execute: fn (cmd cli.Command) ! {
			cmd.execute_help()
		}
		commands: [
			cli.Command{
				name: 'check'
				description: 'Check if system has the required tools installed'
				execute: fn (cmd cli.Command) ! {
					cmd_check()!
				}
			},
			cli.Command{
				name: 'install'
				description: 'Install needed tools'
				execute: fn (cmd cli.Command) ! {
					cmd_install()!
				}
			},
		]
	}
	app.setup()
	app.parse(os.args)
}
