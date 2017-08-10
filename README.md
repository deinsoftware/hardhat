# HardHat [ for Win+Mac ]

I don't like repetitive tasks and make a build is one of them... yuck!

**HardHat** was created to simplify and automate tasks related to Android development.

Previously had create the same app making a [Batch (for Windows)](https://github.com/equiman/hardhatwin/) and a [Bash (for macOS)](https://github.com/equiman/hardhatmac/) scripts to make the task, maintain both of them is a hard task to do, but now with .Net Core can use and share the same code on both Operating Systems.

[![Working Man - Rush](rush-workingman.png)](https://www.youtube.com/watch?v=_-4YOOMqKgk)  
*Rush - Working Man*

Contributions or Beer :beers: will be appreciated :thumbsup:

> The Code is Dark and Full of Errors!  
> Console is your friend ... don't be afraid!

## Menu

* [Getting Started](#getting-started)
  * [Prerequisites](#prerequisites)
  * [Installing](#installing)
* [Environment Variables](#environment-variables)
  * [Windows](#environment-for-windows)
  * [macOS](#environment-for-macos)
* [Usage](#usage)
  * [Permissions](#permissions)
  * [Run](#run)
  * [Keyboard Shortcuts](#keyboard-shortcuts)
  * [Setup](#setup)
* [About](#about)
  * [Built With](#built-with)
  * [Contributing](#contributing)
  * [Versioning](#versioning)
  * [Authors](#authors)
  * [License](#license)
  * [Acknowledgments](#acknowledgments)

---

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites

What things you need to install?

* [Android SDK](https://developer.android.com/studio/index.html#downloads)
* [Gradle](https://gradle.org/install)
* [Git](https://git-scm.com/downloads)
* [Gulp](http://gulpjs.com/) (to Minify and Uglify)
* [Java](http://www.oracle.com/technetwork/java/javase/downloads/index.html)
* [.Net Core](https://www.microsoft.com/net/download/core#/runtime) (optional)
* [Node.js](https://nodejs.org/en/download/) (with NPM)

### Installing

Follow this steps to install on your local machine

Clone **HardHat** from GitHub on *recommended* path. Using this command on terminal:

| OS | Command |
| --- | --- |
| win | `git clone https://github.com/equiman/hardhat.git "D:\Applications\HardHat"` |
| mac | `git clone https://github.com/equiman/hardhat.git ~/Applications/HardHat/` |

## Environment Variables

Environment variables are, in short, variables that describe the environment in which programs run in.

Please verify that you have been configured all correctly. Paths in descriptions are examples (recommended) but use your own paths.

### Environment for Windows

| var | description |
| --- | --- |
| `ANDROID_HOME` | D:\Applications\Android\SDK |
| `ANDROID_NDK_HOME` | %ANDROID_HOME%\ndk-bundle |
| `ANDROID_BT_VERSION` | 26.0.1 |
| `ANDROID_TEMPLATE` | D:\Applications\Android\Studio |
| `CODE_HOME` | C:\Program Files (x86)\Microsoft VS Code |
| `GIT_HOME` | C:\Program Files\Git |
| `GRADLE_HOME` | D:\Applications\Android\Gradle |
| `GULP_PROJECT` | D:\Applications\Gulp |
| `JAVA_HOME` | C:\Program Files\Java\jdk1.8.0_74 |
| `NPM_HOME` | C:\Users\\`user`\AppData\Roaming\npm |
| `VPN_HOME` | C:\Program Files (x86)\CheckPoint\Endpoint Connect |
| `PATH` | %ANDROID_HOME%\build-tools\\%ANDROID_BT_VERSION%;<br>%ANDROID_HOME%\platform-tools;<br>%ANDROID_HOME%\tools;<br>%CODE_HOME%\bin;<br>%GIT_HOME%\cmd;<br>%GRADLE_HOME%\bin;<br>%NPM_HOME%;<br>C:\ProgramData\Oracle\Java\javapath;<br>C:\Program Files (x86)\nodejs\; |

Replace `user` with your windows user name and `ANDROID_BT_VERSION` with your Android SDK Build Tool version (recommended use the last one).

> **Where are environment variables?**  
> In the System Properties window, click on the Advanced tab, then click the Environment Variables button near the bottom of that tab. In the Environment Variables window, highlight the Path variable in the "System variables" section and click the Edit button.

### Environment for macOS

```bash
export ANDROID_HOME="/usr/local/opt/android-sdk/"
export ANDROID_NDK_HOME="/usr/local/opt/android-sdk/ndk-bundle"
export ANDROID_BT_VERSION="25.0.3"
export ANDROID_TEMPLATE="~/Applications/Android/Studio"
export JAVA_HOME="$(/usr/libexec/java_home -v 1.8)"
export GULP_PROJECT="~/Applications/Gulp"
export VPN_HOME="/Applications/CheckPoint/Endpoint Connect"

export PATH="/opt/local/bin:/opt/local/sbin:$PATH"
export PATH="$ANDROID_HOME/build-tools/$ANDROID_BT_VERSION:$PATH"
export PATH="$ANDROID_HOME/platform-tools:$PATH"
export PATH="$ANDROID_HOME/tools:$PATH"
export PATH="$ANDROID_NDK_HOME/:$PATH"
```

> **Where are environment variables?**  
> First, one thing to recognize about OS X is that it is built on Unix. This is where the .bash_profile comes in. When you start the Terminal app in OS X you get a bash shell by default. The bash shell comes from Unix and when it loads it runs the .bash_profile script. You can modify this script for your user to change your settings. This file is located at: `~/.bash_profile`

⇧ [Back to menu](#menu)

---

## Usage

Choose desired letter combination and let **HardHat** work for you. No more tedious and repetitive tasks stealing your precious time.

### Permissions

Before run **HardHat** macOS users need add execute permission to:
`chmod +x mac.sh`, `chmod +x mac.command` and `chmod +x dist/cmd.mac.sh`

### Run

In order to run **HardHat** open a terminal and run this command:

| OS | Path | Command |
| --- | --- | --- |
| win | `"D:\Applications\HardHat"` | `win.bat` |
| mac | `~/Applications/HardHat/` | `sh mac.sh` |

### Start Menu Icon

It's not mandatory but it's highly recommend create and Start Menu icon.

#### Star Menu for Windows

Use this commands on terminal (as Admin) to add an icon on your start menu:

```dos
:: Make Dir
md "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Android"
:: Copy Shortcut
cd /d "D:\Applications\HardHat\"
xcopy "HardHat.lnk" "C:\ProgramData\Microsoft\Windows\Start Menu\Programs\Android"
```

Open **star menu** and over the Android section you will can see the link. Remember mark with 'pin to start' option if you are using Win10.

#### Star Menu for macOS

Select `mac.command` file, then choose `File > Make Alias` or press `Command-L` and name it as **HardHat**.

Copy the picture in `icon.png` file  to the Clipboard.

One way to do this is to open the picture in Preview, choose `Edit > Select All`, then choose `Edit > Copy` or press `Command-C`.

Select **HardHat** (alias shortcut) file, then choose `File > Get Info`. At the top of the Info window, click the picture of the icon to select it, then choose `Edit > Paste` or press `Command-V`.

Just drag and drop **HardHat** (alias shortcut) to your Dock or Desktop.

### Keyboard Shortcuts

> **UPPERCASE** options means default choice in a question, feel free to continue quickly with <kbd>RETURN</kbd> key :wink:

![HardHat Main Menu](hardhat-main.png "HardHat Main Menu")

#### Project

| combination | action |
| --- | --- |
| <kbd>p</kbd> | Select a project inside `path.dir/bsn/prd` path (_see [Setup > Path Variables](#path-variables) section_) that starts with `flt` folder name. This project required to have an `android.prj` folder inside. |
| <kbd>p</kbd>+<kbd>f</kbd> | Select an APK file generated inside selected project on `android.bld` path  with `android.ext` extension name (_see [Setup > Android Variables](#android-variables) section_). |
| <kbd>p</kbd>+<kbd>i</kbd> | Install selected file on an Android device. |
| <kbd>p</kbd>+<kbd>d</kbd> | Make a copy of selected file and choose a new name. |
| <kbd>p</kbd>+<kbd>p</kbd> | Show path and full path about selected file. Copy this paths to clipboard. |
| <kbd>p</kbd>+<kbd>s</kbd> | Show signature information about selected file. |
| <kbd>p</kbd>+<kbd>v</kbd> | Show full information and values about selected file. |

#### Version Control System

| combination | action |
| --- | --- |
| <kbd>v</kbd> | Show current GIT branch. |
| <kbd>v</kbd>+<kbd>d</kbd> | Launch `reset` command over selected project. |
| <kbd>v</kbd>+<kbd>p</kbd> | Launch `pull` command over selected project. |
| <kbd>v</kbd>+<kbd>c</kbd> | Launch `clean` command over selected project and delete unversioned files over selected project. |
| <kbd>r</kbd>+<kbd>d+p</kbd> | Launch `reset` and `pull` command over selected project. |
| <kbd>r</kbd>+<kbd>c+p</kbd> | Launch `clean` and `pull` command over selected project. |

#### Gulp

| combination | action |
| --- | --- |
| <kbd>g</kbd> | Select and show BrowserSync development server configuration. |
| <kbd>g</kbd>+<kbd>u</kbd> | Make a copy of project files (with an additional backup) and launch `gulp build` command over selected project to `GULP_PROJECT` (_see [Environment Variables](#environment-variables) section_). |
| <kbd>g</kbd>+<kbd>r</kbd> | Revert original files to selected project. |
| <kbd>g</kbd>+<kbd>s</kbd> | Start server according to previous configuration. |

Gulp Uglify process was create under `build` task and configured to use some folders. We recommend follow the same structure.

| folder | description |
| --- | --- |
| `bkp` | Backup files |
| `bld` | Result from magic |
| `www` | Project files |

Gulp Browser process was created under `default` task and follow this command help:
`gulp [default] --pth path_value [--int internalpath value] --dmn dimension_value [--flv flavor_value --srv server_number --sync Y/N --host ip_value --ptc http/https --os os_name]`

| parameter | description |
| --- | --- |
| `pth` | Selected project path |
| `dmn` | Server configuration file name under server folder |
| `flv` | Flavor |
| `flv` | Server Number (if have multiple servers with same flavor) |
| `sync` | Enable or disable browserSync task |
| `host` | External IP address access |

| parameter | description |
| --- | --- |
| `pth` | Selected project path |
| `ipt` | web files path inside `pth` |
| `dmn` | Server configuration file name under `server` folder |
| `flv` | Flavor **A**lfa/**B**eta/**S**tag/**P**rod.|
| `srv` | Server Number (if have multiple servers with same flavor) |
| `sync` | (**Y**) Enable or (**N**) Disable Browser Sync. |
| `host` | External IP address access |
| `ptc` | **http** or **https**. |
| `os` | **win** or **mac**. |

#### Build

| combination | action |
| --- | --- |
| <kbd>b</kbd> | Configure your build type, flavor and dimensions. Dimensions can be empty. |
| <kbd>b</kbd>+<kbd>p</kbd> | Copy pre-configured files inside `ANDROID_TEMPLATE` (_see [Environment Variables](#environment-variables) section_) folder and copy inside `android.prj` folder in selected project (_see [Setup > Android Variables](#android-variables) section_). |
| <kbd>b</kbd>+<kbd>c</kbd> | Make `clean` project with gradle command line. |
| <kbd>b</kbd>+<kbd>g</kbd> | Make `clean` and `build` project with gradle command line. |

If you have some pre-configured files to be copied to project path, add it on `ANDROID_TEMPLATE` path (_see [Environment Variables](#environment-variables) section_). Files like:

* local.properties
* gradle.properties
* keystore/development.properties
* keystore/production.properties

#### Android Debug Bridge

| combination | action |
| --- | --- |
| <kbd>a</kbd>+<kbd>r</kbd> | Kill and Restart ADB server. |
| <kbd>a</kbd>+<kbd>d</kbd> | Show device/emulator list. |
| <kbd>a</kbd>+<kbd>w</kbd> | Make a ADB device dis/connection over Wifi. |

#### Extra

| combination | action |
| --- | --- |
| <kbd>c</kbd> | Configuration. |
| <kbd>i</kbd> | Show information about commands version. |
| <kbd>e</kbd> | Show information about environmental variables. |
| <kbd>x</kbd> | Exit application, save progress and close terminal window. |

### Setup

Choose <kbd>c</kbd> _Configuration_ option on main menu and set the values.

#### Path Variables

| var | description |
| --- | --- |
| `dir` | Development path |
| `bsn` | Business folder inside `dir` path  |
| `prd` | Project folder inside `bsn` path |
| `flt` | Filter name folder for projects list |

#### Android Variables

| var | description |
| --- | --- |
| `prj` | Android folder name inside selected project |
| `bld` | Build path inside `prj` path |
| `ext` | APK extension name |
| `cmp` | Files path inside `prj` path to be compacted with gulp |
| `flt` | Filter extension name to be compacted with gulp |

#### Gulp Variables

| var | description |
| --- | --- |
| `srv` | Server folder name inside `GULP_PROJECT` |
| `ext` | Filter extension name for server configuration |

#### Gulp Variables

| var | description |
| --- | --- |
| `snm` | Site name for `VPN_HOME` |

⇧ [Back to menu](#menu)

---

## About

### Built With

* [VS Code](https://code.visualstudio.com/) - Code editing redefined.

### Contributing

Please read [CONTRIBUTING](CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

### Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [HardHat](https://github.com/equiman/hardhat/tags) on GitHub.

### Authors

* **Camilo Martinez** [[Equiman](http://stackoverflow.com/story/equiman)]

See also the list of [contributors](https://github.com/equiman/hardhat/contributors) who participated in this project.

### License

This project is licensed under the GNU GPLv3 License - see the [LICENSE](LICENSE) file for details.

### Acknowledgments

* Beta testers: [Ricardo Mesa](https://github.com/rmesaf) and [Sebastian Loaiza](https://github.com/slmartinez).
* [StackOverflow](http://stackoverflow.com) - The largest online community for programmers.

⇧ [Back to menu](#menu)