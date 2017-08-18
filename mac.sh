#!/bin/bash
clear
resize -s 27 88
clear
fxUpdate

function fxUpdate() {
    clear;
    echo "===================================================================================================="
    echo " UPDATE "
    echo "===================================================================================================="
    echo "";

    echo " --> Updating... "
    updated="$(git pull)"

    if [ "${updated}" != "Already up-to-date." ]; then 
        fxUpdate
        echo ""
        echo "===================================================================================================="
        echo ""
        echo "HardHat was updated please RESTART to continue."
        echo ""
        echo "Refer to CHANGELOG file for details"
        echo "or visit http://www.github.com/equiman/hardhat/"
        echo ""
        echo "===================================================================================================="
        pause "Press [Enter] key to continue..."
        fxExit
    else
        fxStart
    fi
}

function fxUpdate() {
    clear
    git config --local core.filemode false
    git reset --hard HEAD
    git clean -f -d -x
    git pull
    clear
}

function fxStart() {
    cd ~/Applications/HardHat/
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
}

function fxExit() {
    clear
    exit
}

function pause() {
    echo ""
    read -p " $*"
}

