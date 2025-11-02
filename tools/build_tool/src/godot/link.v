module godot

import os

fn link(path string) ! {
	println('Creating links...')
	//
	wd := os.getwd()
	//
	create_path('${path}/addons')!
	create_symlink('${wd}/lavos/addons', '${path}/addons/${symlink_rookies}')!
}

fn create_path(path string) ! {
	if os.is_dir(path) {
		return
	}
	//
	print('Creating folder: ${path}')
	os.mkdir_all(path) or {
		println(' error')
		return err
	}
	println(' ok')
}

fn create_symlink(symlink_source string, symlink_target string) ! {
	println('Creating symlink')
	println('\t source: ${symlink_source}')
	println('\t target: ${symlink_target}')
	os.symlink(symlink_source, symlink_target) or {
		println(' error')
		return err
	}
	//
	println(' ok')
}
