$executePath = "C:\Path\To\MongoBackup.ps1"
$taskName = "JetstreamDB autmated backup"
$taskDescription = "This creates a backup of the JetstreamDB database"

$action = New-ScheduledTaskAction `
  -Execute 'C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe' `
  -Argument "-NoProfile -NoLogo -NonInteractive -ExecutionPolicy Bypass -File $executePath"

$trigger = New-ScheduledTaskTrigger -Daily -At 1am

$taskSettings = New-ScheduledTaskSettingsSet

Register-ScheduledTask `
  -TaskName $taskName `
  -Action $action `
  -Trigger $trigger `
  -Settings $taskSettings `
  -Description $taskDescription