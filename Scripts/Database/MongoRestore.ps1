# Open Dialog to choose the backup directory and restore the database using mongorestore command
Add-Type -AssemblyName System.Windows.Forms
$dialog = New-Object System.Windows.Forms.FolderBrowserDialog
Write-Host "Choose a Backup Directory for the Restore - New Folder Browser Dialog is opened."
$dialog.Description = "Choose a Backup Directory for the Restore"
$dialog.ShowNewFolderButton = $false
$result = $dialog.ShowDialog()

if ($result -eq [System.Windows.Forms.DialogResult]::OK)
{
    $backupDir = $dialog.SelectedPath
    Write-Host "Choosen backup directory: $backupDir"

    # Set the path to the mongorestore command. If you have mongorestore in your PATH, you can leave it as it is.
    $mongorestorePath = "mongorestore"
    $dbHost = "localhost"
    $dbPort = "27017"
    $dbName = "JetstreamDB" # Name of the database to restore
    $username = "admin" # Change this to your actual username
    $password = "password" # Change this to your actual password
    $authenticationDatabase = "admin" # Your authentifications database

    # DISCAIMER: If you are using the authentication, please uncomment the following line and comment the line after that.
    $restoreCommand = "$mongorestorePath --host $dbHost --port $dbPort -u $username -p $password --authenticationDatabase $authenticationDatabase --db $dbName --drop $backupDir"
    # Restore without authentification
    #$restoreCommand = "$mongorestorePath --host $dbHost --port $dbPort --nsInclude=$dbName.* --drop $backupDir"

    # Run the command
    Invoke-Expression $restoreCommand

    Write-Host "Database restored successfully."
}
else
{
    Write-Host "No backup directory selected. Exiting..."
}
