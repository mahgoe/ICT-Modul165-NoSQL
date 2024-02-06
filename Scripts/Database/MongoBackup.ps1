#region Variables

# MongoDB access variables
$dbName = "JetstreamDB"
# $username = ""
# $password = ""
# $authenticationDatabase = ""
$mHost = "localhost"
$port = "27017"

# Folders location and name
$backupPath = "C:\Path\To\Save\Backups"
$currentDate = get-date -format yyyyMMddHHmm
$directoryName = "$dbName-$currentDate"
$directoryPath = Join-Path $backupPath $directoryName

#endregion

$watch = New-Object System.Diagnostics.StopWatch
$watch.Start()
Write-Host "Backup the Database: '$dbName' on local directory: $backupPath."

#region Backup Process (Authorization)
# If you need the database authorization use the command below
<# 
mongodump -h "$mHost" `
   -d "$dbName" `
   -u "$username" `
   -p "$password" `
   --authenticationDatabase "$authenticationDatabase" `
   -o "$directoryPath"
#>
# Or this command if mongodump is not in the System PATH
<# 
C:\Path\To\Tools\mongodump.exe -h "$mHost" `
   -d "$dbName" `
   -u "$username" `
   -p "$password" `
   --authenticationDatabase "$authenticationDatabase" `
   -o "$directoryPath"
#>


#endregion

# Backup Process (Without autorization)
mongodump --host $mHost --port $port --db $dbName -o "$directoryPath"

# DISCLAIMER: Use the absolute path if mongodump is not in the System PATH
# C:\Path\To\Tools\mongodump.exe --host $mHost --port $port --db $databaseName -o "$directoryPath"

Write-Host "Creating the backup for $dbName..."

$watch.Stop();
Write-Host "MongoDB backup completed in "$watch.Elapsed.ToString()