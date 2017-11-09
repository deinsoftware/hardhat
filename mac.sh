#!/bin/bash

function fxStart() {
    #Resize
    clear
    resize -s 29 88
    clear
    #Git
    git config --local core.filemode false
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
        echo " HardHat was updated."
        echo ""
        echo " Refer to CHANGELOG file for details"
        echo " or visit http://www.github.com/equiman/hardhat/"
        echo ""
        echo "======================================================================================="
        pause "Press [Enter] key to continue..."
        fxIsRunning
    else
        fxIsRunning
    fi
}

function fxGit() {
    clear
    git reset --hard HEAD
    git pull
}

function fxDebug() {
    dotnet run
    fxExit
}

function fxIsRunning() {
    number="$(ps aux | grep ./HardHat | wc -l)"

    if [ $number -gt 0 ]; then
        fxStop
    else
        fxRun
    fi
}

function fxRun() {
    chmod +x HardHat
    ./HardHat
    fxExit
}

function fxStop() {
    clear;
    echo "======================================================================================="
    echo " ERROR "
    echo "======================================================================================="
    echo ""
    echo " HardHat is already running."
    echo ""
    echo "======================================================================================="
    pause "Press [Enter] key to continue..."
    fxExit
}

function fxExit() {
    clear
    exit
}

function pause() {
    echo ""
    read -p " $*"
}

fxStart "$1"