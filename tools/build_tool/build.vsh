#!/usr/bin/env -S v run

import os

const wd = os.dir(@FILE)
const output_name = 'lbt'
const binary_path = './bin/'
const src_path = './src'
const cmd = 'v -prod -o ${wd}/${binary_path}/${output_name} ${wd}/${src_path}'

res := execute(cmd)
if res.exit_code != 0 {
	println(res.output)
}
