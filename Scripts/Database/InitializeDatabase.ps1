#region Variables

# MongoDB Connection
$mongoHost = "localhost"
$mongoPort = "27017"
$mongoDbName = "JetstreamDB"

#endregion

#region Initialize Database

# Path to the scripts (AccessControl.js is optional and can be uncommented if needed)
$schemaValidationScript = "C:\Path\To\SchemaValidation.js"
$indexScript = "C:\Path\To\Index.js"
# $accessControlScript = "C:\Path\To\AccesControl.js"

# mongo shell command, need to be added to the PATH environment variable or change the path to the mongosh executable
$mongoShell = "mongosh"

# Executing the scripts one by one
& $mongoShell "${mongoHost}:${mongoPort}/${mongoDbName}" $schemaValidationScript
& $mongoShell "${mongoHost}:${mongoPort}/${mongoDbName}" $indexScript
# & $mongoShell "${mongoHost}:${mongoPort}/${mongoDbName}" $accessControlScript

Write-Host "All scripts executed successfully!"
