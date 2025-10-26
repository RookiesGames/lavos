module actions

import os

const symlink_rookies = 'rookies'

pub fn clean(path string) ! {
	println('Cleaning project...')
	remove_symlink('${path}/addons/${symlink_rookies}')!
	println('Cleanig complete')
}

fn remove_symlink(link string) ! {
	print('\tRemoving symlink ${link}')
	//
	if os.is_file(link) || os.is_link(link) {
		os.rm(link) or {
			println(' error')
			return err
		}
	} else if os.is_dir(link) {
		os.rmdir(link) or {
			println(' error')
			return err
		}
	}
	//
	println(' success')
}
