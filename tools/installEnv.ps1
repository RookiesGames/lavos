$cmdName = "v"
if (Get-Command $cmdName -ErrorAction SilentlyContinue) {
    v up
    exit 0
}
else {
    ""
    "$cmdName does not exist"
    "Head to vlang.io to install it manually"
    exit 1
}