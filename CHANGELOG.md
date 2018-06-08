# Change log

<!-- http://keepachangelog.com/en/0.3.0/ 
Added       for new features.
Changed     for changes in existing functionality.
Deprecated  for once-stable features removed in upcoming releases.
Removed     for deprecated features removed in this release.
Fixed       for any bug fixes.
Security    to invite users to upgrade in case of vulnerabilities.
-->

## [6.0.2] - 2018-06-08

**Changed:**

* Updated library references for [Colorify](https://github.com/equiman/colorify) 1.0.5.

## [6.0.1] - 2018-06-08

**Fix:**

* Error when select web server protocol

## [6.0.0] - 2018-06-08

**Added:**

* Theme selector in Config option.

**Changed:**

* Entire configuration menu, file and options. This change delete your current saved configuration.

**Fix:**

* Error when configuration file can't load saved values. [\#20](https://github.com/equiman/hardhat/issues/20)

## [5.2.1] - 2018-06-07

**Fix:**

* Gulp Uglify error when www folder don't exists. [\#21](https://github.com/equiman/hardhat/issues/21)

**Changed:**

* Updated library references for [ToolBox](https://github.com/equiman/toolbox) 1.1.4 and [Colorify](https://github.com/equiman/colorify) 1.0.4.

## [5.2.0] - 2018-04-30

**Added:**

* Copy properties configuration per dimension. [\#23](https://github.com/equiman/hardhat/issues/23)

## [5.1.1] - 2018-04-30

**Fixed:**

* Internal options menu.

## [5.1.0] - 2018-02-15

**Added:**

* ADB option to change listening port on device connected to USB.

**Fixed:**

* ADB WiFi disconnect status.

## [5.0.2] - 2018-02-05

**Changed:**

* Sonar Scanner run in external terminal window.

## [5.0.1] - 2018-01-04

**Fixed:**

* Update [ToolBox](https://github.com/equiman/toolbox) and [Colorify](https://github.com/equiman/colorify) library, that solves bug with text on resize window.

## [5.0.0] - 2017-12-28

**Changed:**

* Replace internal classes with [ToolBox](https://github.com/equiman/toolbox) and [Colorify](https://github.com/equiman/colorify) libraries.

## [4.3.2] - 2017-12-14

**Added:**

* VPN connection verification on Gulp log option.
* Install dependencies on Gulp Update.

**Fixed:**

* Gulp update check message.

## [4.3.1] - 2017-12-11

**Fixed:**

* Gulp update check.

## [4.3.0] - 2017-12-07

**Added:**

* Gulp log options.

## [4.2.1] - 2017-11-23

**Fixed:**

* Space between label and selected values on main menu.

## [4.2.0] - 2017-11-22

**Added:**

* Gulp make option.

## [4.1.7] - 2017-11-25

**Fixed:**

* Add white theme for mac. [\#14](https://github.com/equiman/hardhat/issues/14)

## [4.1.6] - 2017-11-23

**Removed:**

* SonarLint is no longer supported.

## [4.1.5] - 2017-11-22

**Added:**

* Gulp server parameter optimization.

**Fixed:**

* Check Gulp for Update.

## [4.1.4] - 2017-11-21

**Fixed:**

* Configuration name on empty value. [\#17](https://github.com/equiman/hardhat/issues/17)

## [4.1.3] - 2017-11-16

**Fixed:**

* Production flavor name.

## [4.1.2] - 2017-11-16

**Fixed:**

* Load macOS environment variables on Launch.

## [4.1.1] - 2017-11-09

**Fixed:**

* Gulp update redirection fix.
* One instance running check from startup.

**Changed:**

* Build Tools update message now only works as alert.

## [4.1.0] - 2017-11-08

**Added:**

* Update notification for Gulp project. [\#3](https://github.com/equiman/hardhat/issues/3)
* Open parameter on Gulp server configuration.

**Fixed:**

* Ascending order name on File and Directory list.
* Last version check on build tools.

## [4.0.1] - 2017-11-01

**Added:**

* Android Build Tools check and upgrade version process. [\#11](https://github.com/equiman/hardhat/issues/11)

## [4.0.0] - 2017-10-25

**Added:**

* SonarQube section. [\#2](https://github.com/equiman/hardhat/issues/2)
* Warning alert on Sonar, Gulp or Build incorrect configuration.

**Change:**

* Command to clone (install) project different on each Operating System. Decreasing installed project size by half.
* Whole configuration file, menu and his status validations.
* Update verification don't need restart when download new version.

**Fixed:**

* Copy path and full path on Windows. [\#16](https://github.com/equiman/hardhat/issues/16)

## [3.2.0] - 2017-10-13

**Added:**

* SignCheck for Windows and get SHA256 value from APK option. [\#13](https://github.com/equiman/hardhat/issues/13)

**Fixed:**

* Option to remove selected device. [\#12](https://github.com/equiman/hardhat/issues/12)
* Better code quality and fixes, due SonarQube analysis.

**Changed:**

* Environment variable from `ANDROID_TEMPLATE` to `ANDROID_PROPERTIES`.
* Path from `Applications/Android/Studio` to `Applications/Android/Properties`.
* Reorder Android Debug Bridge menu.

## [3.1.0] - 2017-08-25

**Added:**

* Desk flavor for Gradle and Gulp.
* Auto execute permissions on macOS.

**Changed:**

* Upgrade from .Net Core 1.1 to 2.0.

## [3.0.5] - 2017-08-23

**Fixed:**

* Loop on device detection when ADB server is not running.
* Path index on Uglify process.
* Start menu on macOS.

## [3.0.4] - 2017-08-18

**Removed:**

* Auto update from c#.

**Fixed:**

* Duplicate file option.
* Auto update moved to batch/bash files in order to avoid deadlock.

## [3.0.3] - 2017-08-17

**Removed:**

* Made a project copy to `bkp` folder is unnecessary on Gulp Uglify process, with `www` folder can be restored to original state.

**Added:**

* Launch app on device when installation was success.

**Fixed:**

* Auto update loop.
* When no detect devices, clean selected device.

## [3.0.2] - 2017-08-14

**Fixed:**

* Auto update with total revert.

**Changed:**

* Config file path changed.

## [3.0.1] - 2017-08-09

**Fixed:**

* Batch/Bash exit code 0 to external window command.
* Auto update with stash validation.

**Changed:**

* Default setting file values at fresh start.

## [3.0.0] - 2017-08-09

One code to rule theme all... with .Net Core can share same code for multiple Operating Systems.

This version are merging [HardHat [for Windows]](https://github.com/equiman/hardhatwin/) and [HardHat [for macOS])](https://github.com/equiman/hardhatwin/) projects.

**Added:**

* Configuration, Info and Environment option from main menu.
* Current GIT branch information.

**Changed:**

* Mobile IP Address base from current IP Address.

**Removed:**

* Networks configuration list.
* Subversion as option for Version Control System.
* Apache Server options.
