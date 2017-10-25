# Change log

<!-- http://keepachangelog.com/en/0.3.0/ 
Added       for new features.
Changed     for changes in existing functionality.
Deprecated  for once-stable features removed in upcoming releases.
Removed     for deprecated features removed in this release.
Fixed       for any bug fixes.
Security    to invite users to upgrade in case of vulnerabilities.
-->

## [4.0.0] - 2017-10-25

### Added

* SonarQube section. [\#2](https://github.com/equiman/hardhat/issues/2)
* Warning alert on Sonar, Gulp or Build incorrect configuration.

### Change

* Command to clone (install) project different on each Operating System. Decreasing installed project size by half.
* Whole configuration file, menu and his status validations.

### Fixed

* Copy path and full path on Windows. [\#16](https://github.com/equiman/hardhat/issues/16)

## [3.2.0] - 2017-10-13

### Added

* SignCheck for Windows and get SHA256 value from APK option. [\#13](https://github.com/equiman/hardhat/issues/13)

### Fixed

* Option to remove selected device. [\#12](https://github.com/equiman/hardhat/issues/12)
* Better code quality and fixes, due SonarQube analysis.

### Changed

* Environment variable from `ANDROID_TEMPLATE` to `ANDROID_PROPERTIES`.
* Path from `Applications/Android/Studio` to `Applications/Android/Properties`.
* Reorder Android Debug Bridge menu.

## [3.1.0] - 2017-08-25

### Added

* Desk flavor for Gradle and Gulp.
* Auto execute permissions on macOS.

### Changed

* Upgrade from .Net Core 1.1 to 2.0.

## [3.0.5] - 2017-08-23

### Fixed

* Loop on device detection when ADB server is not running.
* Path index on Uglify process.
* Start menu on macOS.

## [3.0.4] - 2017-08-18

### Removed

* Auto update from c#.

### Fixed

* Duplicate file option.
* Auto update moved to batch/bash files in order to avoid deadlock.

## [3.0.3] - 2017-08-17

### Removed

* Made a project copy to `bkp` folder is unnecessary on Gulp Uglify process, with `www` folder can be restored to original state.

### Added

* Launch app on device when installation was success.

### Fixed

* Auto update loop.
* When no detect devices, clean selected device.

## [3.0.2] - 2017-08-14

### Fixed

* Auto update with total revert.

### Changed

* Config file path changed.

## [3.0.1] - 2017-08-09

### Fixed

* Batch/Bash exit code 0 to external window command.
* Auto update with stash validation.

### Changed

* Default setting file values at fresh start.

## [3.0.0] - 2017-08-09

One code to rule theme all... with .Net Core can share same code for multiple Operating Systems.

This version are merging [HardHat [for Windows]](https://github.com/equiman/hardhatwin/) and [HardHat [for macOS])](https://github.com/equiman/hardhatwin/) projects.

### Added

* Configuration, Info and Environment option from main menu.
* Current GIT branch information.

### Changed

* Mobile IP Address base from current IP Address.

### Removed

* Networks configuration list.
* Subversion as option for Version Control System.
* Apache Server options.
