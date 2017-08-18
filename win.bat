echo off
cls
set dbg=%1

:update
call color 06
cls
echo ==========================================================================================
echo  UPDATE
echo ==========================================================================================
echo. 
echo ==^> Updating...
cd /d %~dp0
git pull | findstr /c:"Already up-to-date"
if %errorlevel% == 0 (
    goto start
) else (
    call color E0
    git config --local core.filemode false
    git reset --hard HEAD
    git clean -f -d -x
    git pull
    cls
    echo. 
    echo ==========================================================================================
    echo. 
    echo  HardHat was updated please RESTART to continue.
    echo. 
    echo  Refer to CHANGELOG file for details
    echo  or visit https://github.com/equiman/hardhatwin/
    echo. 
    echo ==========================================================================================
    echo.
    pause
    goto end
)

:start
if defined dbg (
    cd dev
    dotnet restore
    dotnet run
) else (
    cd dist/win
    HardHat.exe
)
cls

:end
cls
call color 07
exit /b