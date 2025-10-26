module actions

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
	print('\tCreating folder: ${path}')
	os.mkdir_all(path) or {
		println(' error')
		return err
	}
	println(' ok')
}

fn create_symlink(symlink_source string, symlink_target string) ! {
	print('\tCreating ${symlink_target} symlink')
	os.symlink(symlink_source, symlink_target) or {
		println(' error')
		return err
	}
	//
	println(' ok')
}
