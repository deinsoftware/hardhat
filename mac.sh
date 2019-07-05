#!/bin/bash

function fxStart() {
    #Resize
    clear
    resize -s 31 88
osascript <<EOF
    tell application "Terminal" to set bounds of front window to {22, 44, 650, 505}
EOF
    clear
    #Git
    git config --local core.filemode false
    #Mode
    if [ -n "$1" ]; then
        #Debug
        fxDebug
    else
        #Release
        fxIsRunning
    fi
    clear
}

function fxUpdate() {
    cd ~/Applications/HardHat/
    clear;
    echo "================================================================================"
    echo " UPDATE "
    echo "================================================================================"
    echo "";
    echo " --> Updating... "
    echo ""
    updated="$(git pull)"
    if [ "${updated}" == "Already up-to-date." ] || [ "${updated}" == "Already up to date." ] || [ "${updated}" == "Ya está actualizado." ] ; then
        fxRun
    else
        fxGit
        echo ""
        echo "================================================================================"
        echo ""
        echo " HardHat was updated."
        echo ""
        echo " Refer to CHANGELOG file for details"
        echo " or visit http://www.github.com/equiman/hardhat/"
        echo ""
        echo "================================================================================"
        pause "Press [Enter] key to continue..."
        fxRun
    fi
}

function fxPermission() {
    chmod +x mac.sh
    chmod +x mac.command
    chmod +x cmd.sh
    chmod +x HardHat
    chmod +x "Hard Hat"
}

function fxGit() {
    git reset --hard HEAD
    git pull
}

function fxDebug() {
    dotnet run
    fxExit
}

function fxIsRunning() {
    instance="$(pgrep HardHat)"
    if [ -n "${instance}" ]; then
        fxStop
    else
        fxUpdate
    fi
}

function fxRun() {
    fxPermission
osascript <<EOF
    tell application "Terminal" to do script "source ~/.bash_profile; cd ~/Applications/HardHat/; clear; resize -s 31 88; clear; ./HardHat; clear; exit;"
    tell application "Terminal" to set bounds of front window to {22, 44, 650, 505}
EOF
    fxExit
}

function fxStop() {
    clear;
    echo "================================================================================"
    echo " ERROR "
    echo "================================================================================"
    echo ""
    echo " HardHat is already running."
    echo ""
    echo "================================================================================"
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
