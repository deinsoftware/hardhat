# MacOS

## Package Manager

### Brew

Install [Brew](https://brew.sh) - See [usage](https://docs.brew.sh/FAQ)

```bash
/usr/bin/ruby -e "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/master/install)"
brew update
brew upgrade
brew tap caskroom/cask
brew cleanup
brew doctor
```

### SDK Man

Install [SDK Man](http://sdkman.io) - See [usage](https://sdkman.io/usage)

```bash
curl -s "https://get.sdkman.io" | bash
source "$HOME/.sdkman/bin/sdkman-init.sh"
```

Add permissions to user (or group) under this folders and enclosed items:

```bash
sudo chown -R $(whoami) /usr/local/opt
sudo chown -R $(whoami) /usr/local/share
```

**WARNING:** Add next permission just in case you machine is used by only one user.

```bash
sudo chown -R $(whoami) /usr/local/lib
```

## Install

### Java

```bash
sdk list java
```

Search **oracle** version, like `8.0.222.hs-adpt` on this list and use it at the end of next commands.

```bash
sdk install java 8.0.222.hs-adpt
sdk use java 8.0.222.hs-adpt
sdk default java 8.0.222.hs-adpt
```

### Android SDK

```bash
brew cask install android-sdk
```

### Gradle

```bash
sdk install gradle 4.6
sdk use gradle 4.6
sdk default gradle 4.6
```

### Git

```bash
brew install git
rm '/usr/local/bin/git-cvsserver'
brew link --overwrite git
brew install git-lfs
git lfs install
```

### Node.js

```bash
brew install nvm
mkdir ~/.nvm
chmod +x /usr/local/opt/nvm/nvm.sh
```

Add this environment variables to `~/.bash_profile` and/or `~/.zshrc` file:

```bash
export NVM_DIR="$HOME/.nvm"
#Load NVM and Bash Completion
[ -s "/usr/local/opt/nvm/nvm.sh" ] && . "/usr/local/opt/nvm/nvm.sh"
[ -s "/usr/local/opt/nvm/etc/bash_completion" ] && . "/usr/local/opt/nvm/etc/bash_completion"
```

Install last LTS version

```bash
nvm install --lts
nvm alias default stable
nvm use default
```

### PostgreSQL

```bash
brew install postgresql
```

Add this environment variables to `~/.bash_profile` and/or `~/.zshrc` file:

```bash
export POSTGRESQL_HOME="/usr/local/opt/postgresql"

export PATH="$POSTGRESQL_HOME/bin:$PATH"
export LDFLAGS="-L$POSTGRESQL_HOME/lib"
export CPPFLAGS="-I$POSTGRESQL_HOME/include"
```

Initialize and start server

```bash
brew services start postgresql
initdb /usr/local/var/postgres -E utf8
pg_ctl -D /usr/local/var/postgres -l logfile start
createuser -s postgres

```

## Packages

### NPM

```bash
npm i -g npm
npm i -g gulp
npm i -g eslint
```

### Android

```bash
sdkmanager "emulator"
sdkmanager "ndk-bundle"
sdkmanager "build-tools;28.0.3"
sdkmanager "platforms;android-29"
sdkmanager "platforms;android-28"
sdkmanager "platforms;android-27"
sdkmanager "platforms;android-26"
sdkmanager "platforms;android-25"
sdkmanager "platforms;android-24"
sdkmanager "platforms;android-23"
sdkmanager "platforms;android-22"
sdkmanager "platforms;android-21"
sdkmanager "tools"
```

## Software

### Visual Studio Code

```bash
brew cask install visual-studio-code
```

On first run press `⌘` + `⇧` + `P` and run the command: `Shell Command: install 'code' command PATH`. And you can use `code` command from command line. Example: `code .` open editor on current folder.

## Android Studio

```bash
brew cask install android-studio
brew cask install intel-haxm
```

On first run go to `Tools -> Create Command-line Launcher`. And you can use `studio` command from command line. Example: `studio .` open IDE on current folder.

### Source Tree

```bash
brew cask install sourcetree
```

### Google Chrome

```bash
brew cask install google-chrome
```

### pgAdmin

```bash
brew cask install pgadmin4
```

## Configuration

### SonarQube

With pgAdmin create the `sonarqube` user with `sonarqube` password with all privilegies. 

Run this script to create the DataBase:

```sql
CREATE DATABASE sonar WITH ENCODING 'UTF8' OWNER sonar TEMPLATE=template0;
```

Configure this values on `sonar.properties` file:

```txt
sonar.jdbc.username=sonarqube
sonar.jdbc.password=sonarqube
sonar.jdbc.url=jdbc:postgresql://localhost/sonar
sonar.web.port=9000
```
