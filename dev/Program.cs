using System;
using dein.tools;

namespace HardHat
{
    class Program
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
                
                //Update Environment Variables
                Env.CmdUpdate();
                
                //Window
                if (OS.IsWindows() && (config.window.width + config.window.height) > 0)
                {
                    Console.SetWindowSize(config.window.width, config.window.height);
                }

                VCS.Update();
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