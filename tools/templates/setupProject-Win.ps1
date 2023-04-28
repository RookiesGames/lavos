# Install environment
& "$PSScriptRoot\lavos\tools\installEnv.ps1"

if ($? -eq $false)
{ 
    ""
    "Environment installation failed"
    exit
}

# Setup project
$dir = get-location
$project = "speops"
$path = "$dir\$project"
v run .\lavos\tools\setupProject.vsh -p $path