module helper

import os

pub fn validate_path(path string) !string {
	if !os.is_dir(path) {
		return error('Path ${path} does not exist')
	}
	//
	return path
}
