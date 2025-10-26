#!/usr/bin/env -S v run

const output_name = "lbt"
const binary_path = './bin/'
const src_path = './src'
const cmd = 'v -prod -o ${binary_path}/${output_name} ${src_path}'

execute(cmd)