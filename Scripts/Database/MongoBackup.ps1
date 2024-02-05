# Path to your mongodump.exe. Change this according to your installation.
# Delete Comment if you want to use the full path to mongodump
# $MongodumpPath = "C:\Program Files\MongoDB\Server\7.0\bin\mongodump.exe"

# Directory where the backup will be saved.
$BackupDir = "../Backups"


# Create a timestamp to be used for the backup file name
$TimeStamp = Get-Date -Format "yyyy-MM-dd-HH-mm-ss"

# Create a directory with the timestamp to store the backup
$BackupDBDir = Join-Path -Path $BackupDir -ChildPath ($TimeStamp + "_JetstreamDB")
New-Item -ItemType Directory -Path $BackupDBDir -Force | Out-Null

# Host and Port to your MongoDB server. Change this according to your MongoDB Server Configuration.
$MongoHost = "localhost"
$Port = "27017"

# Database Name
$DbName = "JetstreamDB"

# Start the backup process
# DISCLAIMER: Comment out the line you don't want to use. Default is 2. commented out.
# 1. Direct mongodump if mongodump is in the Windows PATH
mongodump --host $MongoHost --port $Port --db $DbName --out $BackupDBDir
# 2. Use the full path to mongodump
# & $MongodumpPath --host $MongoHost --port $Port --db $DbName --out $BackupDir

# Check if the backup process was successful
Write-Host "Backup for the database '$DbName' was successful created on the directory '$BackupDir'."
