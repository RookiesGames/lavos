#!/usr/bin/env -S v run

import os

const wd = os.dir(@FILE)
const output_name = 'lbt'
const src_path = 'src'

const output_path = '${wd}/bin'
const cmd = 'v -prod -o ${output_path}/lbt ${wd}/src'

if os.exists(output_path) == false {
	os.mkdir(output_path)!
}

res := execute(cmd)
if res.exit_code != 0 {
	println(res.output)
}
