using System;
using Colorify;
using Colorify.UI;
using dein.tools;
using ToolBox.Files;
using ToolBox.Log;
using ToolBox.Platform;
using ToolBox.System;

namespace HardHat
{
    static class Program
    {
        public static Config _config { get; set; }
        public static IFileSystem _fileSystem { get; set; }
        public static DiskConfigurator _disk { get; set; }
        public static PathsConfigurator _path { get; set; }
        public static Format _colorify { get; set; }
        public static ILogSystem _logSystem { get; set; }

        static void Main(string[] args)
        {
            try
            {
                Factory();
                Config();
                Upgrade();

                Menu.Start();
                _colorify.ResetColor();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
                Message.Error(
                    msg: Ex.Message,
                    replace: true,
                    exit: true);
                Exit();
            }
        }

        private static void Factory()
        {
            _fileSystem = FileSystem.Default;
            _disk = new DiskConfigurator(_fileSystem, new ConsoleNotificationSystem());
            switch (OS.GetCurrent())
            {
                case "win":
                    _path = new PathsConfigurator(CommandSystem.Win, _fileSystem);
                    _colorify = new Format(Theme.Dark);
                    break;
                case "mac":
                    _path = new PathsConfigurator(CommandSystem.Mac, _fileSystem);
                    _colorify = new Format(Theme.Light);
                    break;
            }
            _logSystem = new FileLogTxt(_fileSystem, _path.Combine("~"), ".hardhat.log");
        }

        private static void Config()
        {
            _config = Settings.Read();
            _config.personal.hostName = User.GetMachine();
            if (!String.IsNullOrEmpty(_config.personal.theme))
            {
                switch (_config.personal.theme)
                {
                    case "d":
                        _colorify = new Format(Theme.Dark);
                        break;
                    case "l":
                        _colorify = new Format(Theme.Light);
                        break;
                }
            }
        }

        private static void Upgrade()
        {
            Variables.Upgrade();
            Variables.Update();
            Gulp.Check();
        }

        public static void Exit()
        {
            Settings.Save(_config);
            _colorify.ResetColor();
            Environment.Exit(0);
        }
    }
}