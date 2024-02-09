# ICT-Modul165-NoSQL

## Übersicht

Dieses Projekt wurde im Rahmen der ICT-Modul 165 entwickelt in dem es hauptsächlich um die NoSQL Anbindungen geht. Das Backend wurde neu entwickelt und für ein MongoDB vorbereitet und ist funktionstüchtig.

## Technologiestack

- API ist mit ASP.NET Core
- MongoDB als NoSQL Datenbank
- JavaScript + PowerShell für Skripte

# Installation

## Voraussetzungen:

- .NET 8.0 SDK
- MongoDB und alle Mongotools (empfohlen nach der Installation alle Systemvariablen zu setzen)
- PowerShell Skripts müssen ausgeführt werden können.

Das Projekt kann über diesem Repository geklont werden.

## Ersteinrichtung

Im Skriptornder ist ein weiterer Ordner namens `Database`, dies beinhaltet:

- weiterer Ordner mit `SQLMigrations`
  - Dies ist ein Abbild des alten SQL Datenbankes, die Migration findet aber im Backend statt.
- `AccessControl.js`
  - JavaScript, die mehrere Datenbankzugänge erstellt.
- `Index.js`
  - JavaScript, die Indexes erstellt im MongoDB
- `InitializeDatabase.ps1`
  - PowerShell, dass die Erstinitialisierung durchführt. Bitte Pfäde ändern.
- `MongoBackup.ps1`
  - PowerShell, dass ein Backup erstellt. Bitte alle Pfäde ändern
- `MongoRestore.ps1`
  - PowerShell, dass ein Restore eines Backups durchführt. Bitte alle Pfäde ändern
- `README.txt`
  - Dies ist umbedingt zu beachten und zu lesen
- `SchemaValidation.js`
  - JavaScript, dass eine Schemavalidation erstellt im MongoDB.
- `WindowsTaskplanner.js`
  - PowerShell, dass ein automatischen Backup ermöglicht. Es wird ein Aufgabenplanereintrag erstellt.

## Postman Collections

Die Postman Collection beinhaltet alle Endpunkte und ein Testprojekt, die verschiedene Testfälle beinhaltet, die getestet werden.

Wichtig ist, dass die Authentifikationstoken aktuell und korekt sind im `Auth` Tab.

# NuGet Pakete und Versionierungen

| NuGet-Package                                       | Version |
| --------------------------------------------------- | ------- |
| AutoMapper.Extensions.Microsoft.DependencyInjection | 12.0.1  |
| MMicrosoft.AspNetCore.Authentication.JwtBearer      | 8.0.1   |
| Microsoft.IdentityModel.Tokens                      | 7.2.0   |
| MongoDB.Bson                                        | 2.23.1  |
| MongoDB.Driver                                      | 2.23.1  |
| MongoDB.Driver.Core                                 | 2.23.1  |
| Swashbuckle.AspNetCore                              | 6.5.0   |
| System.IdentityModel.Tokens.Jwt                     | 7.2.0   |
| Microsoft.NET.Test.Sdk                              | 17.6.0  |
| Moq                                                 | 4.20.69 |
| Xunit                                               | 2.6.1   |
| xunit.runner.visualstudio                           | 2.4.5   |
