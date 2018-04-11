$RavenDbFile = "RavenDB-4.0.3-patch-40034-windows-x64.zip"
$RavenDbFileDownloadUrl = "https://daily-builds.s3.amazonaws.com/" + $RavenDbFile
$RavenDbDirectory = "$PSScriptRoot\RavenDB"
$RavenDbFilePath = "$RavenDbDirectory\$RavenDbFile"

Write-Output "Downloading $RavenDbFile..."
(New-Object Net.WebClient).DownloadFile($RavenDbFileDownloadUrl, $RavenDbFilePath)

Write-Output "Unzipping to $RavenDbDirectory..."
Expand-Archive -Path $RavenDbFilePath -DestinationPath $RavenDbDirectory
