#!/bin/bash
token="$(cat sonar.txt)"
# https://docs.sonarqube.org/display/SONAR/Analysis+Parameters
dotnet ~/Applications/Sonar/Scanner/msbuild/SonarScanner.MSBuild.dll begin /k:"dein:hardhat" /n:"HardHat" /v:"6.3.0" /o:"dein" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="${token}" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*"
dotnet restore
dotnet build
dotnet test HardHat.Tests/HardHat.Tests.csproj
dotnet ~/Applications/Sonar/Scanner/msbuild/SonarScanner.MSBuild.dll end /d:sonar.login="${token}"