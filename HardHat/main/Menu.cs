using System;
using System.Reflection;
using static Colorify.Colors;
using dein.tools;
using ToolBox.System;
using static HardHat.Program;

namespace HardHat
{

    public static class Menu
    {

        public static void Status(string sel = null)
        {
            try
            {
                _config.personal.ipAddress = Network.GetLocalIPv4();

                if (!String.IsNullOrEmpty(sel))
                {
                    _config.personal.menu.selectedOption = sel;
                }

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
                Project.Status(dirPath);
                Vcs.Status(dirPath);
                Sonar.Status();
                Task.Status();
                Build.Status();
                Configuration.Status();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Start()
        {
            _colorify.Clear();

            string name = Assembly.GetEntryAssembly().GetName().Name.ToUpper().ToString();
            string version = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            Section.Header($" {name} # {version}|{_config.personal.hostName} : {_config.personal.ipAddress} ");

            Status("m");
            Project.Start();
            Vcs.Start();
            Task.Start();
            Build.Start();
            Sonar.Start();
            Adb.Start();

            Section.Footer();

            Section.HorizontalRule();

            _colorify.Write($"{" Make your choice:",-25}", txtInfo);
            string opt = Console.ReadLine().Trim();
            _colorify.Clear();
            Route(opt);
        }

        public static void Route(string sel = "m", string main = "m")
        {
            _config.personal.menu.selectedOption = sel?.ToLower();
            if (!String.IsNullOrEmpty(_config.personal.menu.selectedOption))
            {
                if (Options.IsValid(_config.personal.menu.selectedOption))
                {
                    Action act = Options.GetAction(_config.personal.menu.selectedOption, main);
                    _config.personal.menu.selectedVariant = Options.GetVariant(_config.personal.menu.selectedOption);
                    _config.personal.menu.selectedOption = main;
                    act.Invoke();
                }
                else
                {
                    _config.personal.menu.selectedVariant = "";
                    _config.personal.menu.selectedOption = main;
                    Message.Error();
                }
            }
            else
            {
                Menu.Start();
            }
        }
    }
}