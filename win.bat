echo off
cls
set dbg=%1

if defined dbg (
    cd dev
    dotnet restore
    dotnet run
) else (
    cd dist/win
    HardHat.exe
)
cls