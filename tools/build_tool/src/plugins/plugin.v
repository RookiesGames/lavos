module plugins

interface Plugin {
	build() !
	clean() !
}
