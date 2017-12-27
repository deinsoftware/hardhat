using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using ToolBox.Files;
using ToolBox.System;
using ct = dein.tools.Colorify.Type;
using static HardHat.Program;

namespace HardHat {
    
    public static class Menu {

        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Menu()
        {
            _c  = Program._config;
            _cp = Program._config.personal;
        }

        public static void Status(string sel = null){
            try
            {
                _cp.ipl = Network.GetLocalIPv4();

                if (!String.IsNullOrEmpty(sel))
                {
                    _cp.mnu.sel = sel;
                }

                string dirPath = _path.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr ?? "");
                Project.Status(dirPath);
                Vcs.Status(dirPath);
                Sonar.Status();
                Gulp.Status();
                Build.Status();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void Start() {
            Colorify.Default();

            string name = Assembly.GetEntryAssembly().GetName().Name.ToUpper().ToString();
            string version = Assembly.GetEntryAssembly().GetName().Version.ToString();

            Section.Header($" {name} # {version}|{_cp.hst} : {_cp.ipl} ");

            Status("m");
            Project.Start();
            Vcs.Start();
            Sonar.Start();
            Gulp.Start();
            Build.Start();
            Adb.Start();
            Section.Footer();

            Section.HorizontalRule();

            $"{" Make your choice:", -25}".txtInfo();
            string opt = Console.ReadLine();
            Route(opt);
        }

        public static void Route(string sel = "m", string dfl = "m") {
            _cp.mnu.sel = sel?.ToLower();
            if (!String.IsNullOrEmpty(_cp.mnu.sel)){
                if (Options.Valid(_cp.mnu.sel))
                {
                    Action act = Options.Action(_cp.mnu.sel, dfl);
                    _cp.mnu.sel = dfl;
                    act.Invoke();
                } else {
                    Message.Error();
                }
            } else {
                Menu.Start();
            }
        }
    }
}