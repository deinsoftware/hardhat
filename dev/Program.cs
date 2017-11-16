using System;
using System.Reflection;
using System.Threading;
using dein.tools;

namespace HardHat
{
    static class Program
    {
        public static Config config  { get; set; }

        static void Main(string[] args)
        {
            try
            {
                //Config
                config = Settings.Read();
                var cp = config.personal;
                cp.hst = System.Environment.MachineName;

                //Window
                if (Os.IsWindows() && (config.window.width + config.window.height) > 0)
                {
                    Console.SetWindowSize(config.window.width, config.window.height);
                }
                
                //Check for updates
                Variables.Upgrade();
                Variables.Update();
                Gulp.Check();
                
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Message.Error(
                    msg: Ex.Message, 
                    replace: true, 
                    exit: true);
                Exit();
            }
        }

        public static void Exit(){
            Settings.Save(config);
            Console.Clear();
            Environment.Exit(0);
        }
    }
}