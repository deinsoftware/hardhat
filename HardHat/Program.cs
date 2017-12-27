using System;
using System.Reflection;
using System.Threading;
using dein.tools;
using ToolBox.Files;
using ToolBox.Platform;
using ToolBox.System;

namespace HardHat
{
    static class Program
    {
        public static Config _config  { get; set; }
        static DiskConfigurator _disk {get; set;}
        static PathsConfigurator _path {get; set;}

        static void Main(string[] args)
        {
            try
            {
                //Factory
                _disk = new DiskConfigurator(FileSystem.Default);
                switch (OS.GetCurrent())
                {
                    case "win":
                        _path = new PathsConfigurator(CommandSystem.Win, FileSystem.Default);
                        break;
                    case "mac":
                        _path = new PathsConfigurator(CommandSystem.Mac, FileSystem.Default);
                        break;
                }
                
                //Config
                _config = Settings.Read();
                var cp = _config.personal;
                cp.hst = User.GetMachine();

                //Window
                if (OS.IsWin() && (_config.window.width + _config.window.height) > 0)
                {
                    Console.SetWindowSize(_config.window.width, _config.window.height);
                }

                //Check for updates
                Variables.Upgrade();
                Variables.Update();
                Gulp.Check();
                
                Menu.Start();
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

        public static void Exit(){
            Settings.Save(_config);
            Console.Clear();
            Environment.Exit(0);
        }
    }
}