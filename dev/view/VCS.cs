using System;
using System.Collections.Generic;
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

        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="vd"  , stt=false, act=Vcs.Discard                      });
            opts.Add(new Option{opt="vp"  , stt=false, act=Vcs.Pull                         });
            opts.Add(new Option{opt="vr"  , stt=false, act=Vcs.Reset                        });
            opts.Add(new Option{opt="vd+p", stt=false, act=Vcs.DiscardPull                  });
            opts.Add(new Option{opt="vr+p", stt=false, act=Vcs.ResetPull                    });
        }

        public static void Status(string dirPath){
            _cp.mnu.v_bnc = "";
            if (!String.IsNullOrEmpty(_cp.spr)){
                string bnc = Git.CmdBranch(dirPath);
                if (!String.IsNullOrEmpty(bnc))
                {
                    _cp.mnu.v_bnc = $"git://{Git.CmdBranch(dirPath)}";
                } 
            }
            Options.Valid("v"   , Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
            Options.Valid("vd"  , Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
            Options.Valid("vp"  , Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
            Options.Valid("vr"  , Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
            Options.Valid("vd+p", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
            Options.Valid("vr+p", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_cp.spr, _cp.mnu.v_bnc));
        }

        public static void Start() {
            if (Options.Valid("v"))
            {
                $" [V] VCS".txtMuted(ct.WriteLine);
            } else {
                $"{" [V] VCS:", -25}".txtMuted(ct.Write);
                $"{_cp.mnu.v_bnc}".txtDefault(ct.WriteLine);
            }
            $"{"   [D] Discard" , -34}".txtStatus(ct.Write,     Options.Valid("vd"));
            $"{"[P] Pull"       , -34}".txtStatus(ct.Write,     Options.Valid("vp"));
            $"{"[R] Reset"      , -17}".txtStatus(ct.WriteLine, Options.Valid("vr"));
            $"".fmNewLine();
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
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }
    }
}