# Setup project
$dir = get-location
$project = "<project_folder>"
$path = "$dir\$project"
v run .\lavos\tools\lavos.vsh -p $path