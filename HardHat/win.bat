@echo off
call refreshenv > nul
cls
set dbg=%1

:start
:: Resize
cls
mode con:cols=86 lines=30
if defined dbg (
    goto debug
) else (
    goto running
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
git pull | findstr /irc:"Already up.to.date"
if %errorlevel% == 0 (
    goto run
) else (
    call color E0
    git config --local core.filemode false
    git reset --hard HEAD
    git pull
    echo. 
    echo ==========================================================================================
    echo. 
    echo  HardHat was updated.
    echo. 
    echo  Refer to CHANGELOG file for details
    echo  or visit https://github.com/equiman/hardhat/
    echo. 
    echo ==========================================================================================
    echo.
    pause
    goto run
)

:debug
call color 07
dotnet run
goto end

:running
set exe=HardHat.exe
for /f %%x in ('tasklist /nh /fi "imagename eq %exe%"') do if %%x == %exe% goto stop
goto update

:run
call color 07
HardHat.exe
goto end

:stop
call color FC
cls
cls
echo ==========================================================================================
echo  ERROR
echo ==========================================================================================
echo. 
echo  HardHat is already running.
echo. 
echo ==========================================================================================
echo.
pause
goto end

:end
cls
call color 07
exit /b