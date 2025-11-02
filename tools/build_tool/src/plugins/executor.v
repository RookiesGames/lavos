module plugins

import os

fn executor(cmd string) ! {
	print('Executing ${cmd}...')
	res := os.execute(cmd)
	if res.exit_code != 0 {
		println(' error')
		return error(res.output)
	}
	//
	println(' success')
}
