# Install environment
& "$PSScriptRoot\lavos\tools\env\installEnv.ps1"

if ($? -eq $false) { 
    ""
    "Environment installation failed"
    exit
}

# Setup project
$dir = get-location
$project = "<project_folder>"
$path = "$dir\$project"
v run .\lavos\tools\setupProject.vsh -p $path