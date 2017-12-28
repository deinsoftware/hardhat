using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using static Colorify.Colors;
using dein.tools;
using ToolBox.Files;
using ToolBox.System;
using static HardHat.Program;

namespace HardHat {
    
    public static class Menu {

        public static void Status(string sel = null){
            try
            {
                _config.personal.ipl = Network.GetLocalIPv4();

                if (!String.IsNullOrEmpty(sel))
                {
                    _config.personal.mnu.sel = sel;
                }

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr ?? "");
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
            _colorify.Clear();

            string name = Assembly.GetEntryAssembly().GetName().Name.ToUpper().ToString();
            string version = Assembly.GetEntryAssembly().GetName().Version.ToString();

            Section.Header($" {name} # {version}|{_config.personal.hst} : {_config.personal.ipl} ");

            Status("m");
            Project.Start();
            Vcs.Start();
            Sonar.Start();
            Gulp.Start();
            Build.Start();
            Adb.Start();
            Section.Footer();

            Section.HorizontalRule();

            _colorify.Write($"{" Make your choice:", -25}", txtInfo);
            string opt = Console.ReadLine();
            _colorify.Clear();
            Route(opt);
        }

        public static void Route(string sel = "m", string dfl = "m") {
            _config.personal.mnu.sel = sel?.ToLower();
            if (!String.IsNullOrEmpty(_config.personal.mnu.sel)){
                if (Options.Valid(_config.personal.mnu.sel))
                {
                    Action act = Options.Action(_config.personal.mnu.sel, dfl);
                    _config.personal.mnu.sel = dfl;
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