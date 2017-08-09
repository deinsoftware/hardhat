using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    class VCS {
        public static void Actions(bool discard, bool pull, bool reset)
        {
            Colorify.Default();
            Console.Clear();

            var c = Program.config;
            var cp = Program.config.personal;
            try
            {
                string sourcePath = Paths.Combine(Env.Get("ANDROID_TEMPLATE"));
                string destinationPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj); 

                $"=".bgInfo(ct.Repeat);
                $" GIT".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);
                
                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr); 

                if (discard) {
                    $"".fmNewLine();
                    $" --> Discarding...".txtInfo(ct.WriteLine);
                    GIT.CmdDiscard(dirPath);
                }

                if (reset){
                    $"".fmNewLine();
                    $" --> Reseting...".txtInfo(ct.WriteLine);
                    GIT.CmdReset(dirPath);
                }
                
                if (pull) {
                    $"".fmNewLine();
                    $" --> Updating...".txtInfo(ct.WriteLine);
                    GIT.CmdPull(dirPath);
                }

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Press [Any] key to continue...".txtInfo();
                Console.ReadKey();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Update(){
            Colorify.Default();
            Console.Clear();

            var c = Program.config;
            var cp = Program.config.personal;
            try
            {
                bool response = GIT.CmdUpdate();
                if(response)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" HardHat was updated please RESTART to continue.");
                    msg.Append(Environment.NewLine);
                    msg.Append(Environment.NewLine);
                    msg.Append($" Refer to CHANGELOG file for details.");
                    msg.Append($" or visit http://www.github.com/equiman/hardhat/");

                    Message.Alert(
                        msg: msg.ToString(),
                        exit: true
                    );
                } else {
                    Menu.Start();
                }
            }
            catch (Exception Ex)
            {
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}