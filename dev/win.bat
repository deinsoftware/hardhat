@echo off
call refreshenv > nul
cls
set dbg=%1

:start
cls
call color 07
if defined dbg (
    goto debug
) else (
    goto update
)
cls

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
    goto run
) else (
    cls
    call color E0
    git config --local core.filemode false
    git reset --hard HEAD
    git pull
    echo. 
    echo ==========================================================================================
    echo. 
    echo  HardHat was updated please RESTART to continue.
    echo. 
    echo  Refer to CHANGELOG file for details
    echo  or visit https://github.com/equiman/hardhat/
    echo. 
    echo ==========================================================================================
    echo.
    pause
    goto end
)

:debug
dotnet run
goto end

:run
HardHat.exe
goto end

:end
cls
call color 07
exit /b