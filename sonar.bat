@echo off
set /p token=<sonar.txt
:: https://docs.sonarqube.org/display/SONAR/Analysis+Parameters
SonarQube.Scanner.MSBuild.exe begin /k:"dein:hardhat" /n:"HardHat" /v:"6.7.2" /o:"dein" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="%token%" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*"
dotnet restore
dotnet build
dotnet test HardHat.Tests/HardHat.Tests.csproj
SonarQube.Scanner.MSBuild.exe end /d:sonar.login="%token%"