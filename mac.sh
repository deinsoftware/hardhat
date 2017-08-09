#!/bin/bash
clear
resize -s 27 88
clear
if [ -n "$1" ]; then
    #Development
    cd dev
    dotnet restore
    dotnet run
else
    #Release
    cd dist/mac
    ./HardHat
fi
clear