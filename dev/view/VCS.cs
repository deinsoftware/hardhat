using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public static class Vcs {
        
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Vcs()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Discard(){
            Vcs.Actions(true, false, false);
        }

        public static void Pull(){
            Vcs.Actions(false, true, false);
        }

        public static void Reset(){
            Vcs.Actions(false, false, true);
        }

        public static void DiscardPull(){
            Vcs.Actions(true, true, false);
        }

        public static void ResetPull(){
            Vcs.Actions(false, true, true);
        }

        public static void Actions(bool discard, bool pull, bool reset)
        {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("GIT");
                Section.SelectedProject();

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr); 

                if (discard) {
                    $"".fmNewLine();
                    $" --> Discarding...".txtInfo(ct.WriteLine);
                    Git.CmdDiscard(dirPath);
                }

                if (reset){
                    $"".fmNewLine();
                    $" --> Reseting...".txtInfo(ct.WriteLine);
                    Git.CmdReset(dirPath);
                }
                
                if (pull) {
                    $"".fmNewLine();
                    $" --> Updating...".txtInfo(ct.WriteLine);
                    Git.CmdPull(dirPath);
                }

                Section.HorizontalRule();

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
    }
}