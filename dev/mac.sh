#!/bin/bash
clear
resize -s 27 88
clear

function fxStart() {
    #Permissions
    chmod +x mac.sh
    chmod +x mac.command
    chmod +x cmd.mac.sh
    if [ -n "$1" ]; then
        #Development
        fxDebug
    else
        fxUpdate
    fi
    clear
}

function fxUpdate() {
    cd ~/Applications/HardHat/
    clear;
    echo "======================================================================================="
    echo " UPDATE "
    echo "======================================================================================="
    echo "";

    echo " --> Updating... "
    updated="$(git pull)"
    if [ "${updated}" != "Already up-to-date." ]; then 
        fxGit
        echo ""
        echo "======================================================================================="
        echo ""
        echo "HardHat was updated."
        echo ""
        echo "Refer to CHANGELOG file for details"
        echo "or visit http://www.github.com/equiman/hardhat/"
        echo ""
        echo "======================================================================================="
        pause "Press [Enter] key to continue..."
        fxRun
    else
        fxRun
    fi
}

function fxGit() {
    git config --local core.filemode false
    git reset --hard HEAD
    git pull
}

function fxDebug() {
    dotnet run
    fxExit()
}

function fxRun() {
    chmod +x HardHat
    ./HardHat
    fxExit()
}

function fxExit() {
    clear
    exit
}

function pause() {
    echo ""
    read -p " $*"
}

fxStart