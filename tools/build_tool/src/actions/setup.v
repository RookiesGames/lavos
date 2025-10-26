module actions

pub fn setup(path string) ! {
	println('Setting up project...')
	//
	check(path)!
	clean(path)!
	link(path)!
}
