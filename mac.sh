#!/bin/bash

function fxStart() {
    #Resize
    clear
    resize -s 31 88
    tell application "Terminal" to set bounds of front window to {22, 44, 650, 535}
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
    if [ "${updated}" == "Already up-to-date." ] || [ "${updated}" == "Already up to date." ] ; then
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
    tell application "Terminal" 
        do script "source ~/.bash_profile"
        do script "cd ~/Applications/HardHat/" in window 1
        do script "clear" in window 1
        do script "resize -s 31 88" in window 1
        do script "clear" in window 1
        do script "./HardHat" in window 1
        do script "clear" in window 1
        do script "exit" in window 1
    end tell
    tell application "Terminal"
        set bounds of front window to {22, 44, 650, 535}
    end tell
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
