using System;
using Colorify;
using static Colorify.Colors;
using Colorify.UI;
using dein.tools;
using ToolBox.Files;
using ToolBox.Platform;
using ToolBox.System;

namespace HardHat
{
    static class Program
    {
        public static Config _config  { get; set; }
        public static DiskConfigurator _disk {get; set;}
        public static PathsConfigurator _path {get; set;}
        public static Format _colorify {get; set;}

        static void Main(string[] args)
        {
            try
            {
                Factory();
                Config();
                Resize();
                Upgrade();
                
                Menu.Start();
                _colorify.ResetColor();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
                Message.Error(
                    msg: Ex.Message, 
                    replace: true, 
                    exit: true);
                Exit();
            }
        }

        private static void Factory(){
            _disk = new DiskConfigurator(FileSystem.Default);
            switch (OS.GetCurrent())
            {
                case "win":
                    _path = new PathsConfigurator(CommandSystem.Win, FileSystem.Default);
                    _colorify = new Format(Theme.Win);
                    break;
                case "mac":
                    _path = new PathsConfigurator(CommandSystem.Mac, FileSystem.Default);
                    _colorify = new Format(Theme.Mac);
                    break;
            }
        }

        private static void Config(){
            _config = Settings.Read();
            _config.personal.hst = User.GetMachine();
        }

        private static void Resize(){
            if (OS.IsWin() && (_config.window.width + _config.window.height) > 0)
            {
                Console.SetWindowSize(_config.window.width, _config.window.height);
            }
        }

        private static void Upgrade(){
            Variables.Upgrade();
            Variables.Update();
            Gulp.Check();
        }
        
        public static void Exit(){
            Settings.Save(_config);
            _colorify.ResetColor();
            Environment.Exit(0);
        }
    }
}