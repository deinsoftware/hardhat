using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Colorify.Colors;
using dein.tools;
using ToolBox.Platform;
using ToolBox.System;
using ToolBox.Validations;
using static HardHat.Program;

namespace HardHat
{

    public static partial class Adb
    {
        public static void Logcat()
        {
            Adb.SelectLogcat();
        }
        public static void SelectLogcat()
        {
            _colorify.Clear();

            try
            {
                Section.Header("LOGCAT");

                Section.SelectedFile();
                Section.SelectedPackageName();

                _colorify.BlankLines();
                _colorify.Write($"{" [N] Package Name:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.logcat.packageName}");
                string logcatPriority = Selector.Name(Selector.Priority, _config.personal.logcat.priority);
                _colorify.Write($"{" [P] Priority:",-25}", txtPrimary); _colorify.WriteLine($"{logcatPriority}");

                _colorify.BlankLines();
                _colorify.Write($"{" [S] Show",-68}", txtPrimary);
                _colorify.WriteLine($"{"[EMPTY] Cancel",-17}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:",-25}", txtInfo);
                string opt = Console.ReadLine()?.ToLower();

                if (String.IsNullOrEmpty(opt))
                {
                    Menu.Start();
                }
                else
                {
                    Menu.Route($"al>{opt}", "al");
                }
                Message.Error();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void PackageName()
        {
            _colorify.Clear();

            try
            {
                Section.Header("LOGCAT", "PACKAGE NAME");

                Section.SelectedFile();
                Section.SelectedPackageName();

                _colorify.BlankLines();
                _colorify.WriteLine($" Write Application Package Name.", txtPrimary);
                _colorify.WriteLine($" Avoid using wildcards.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice:",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                _config.personal.logcat.packageName = opt;

                Menu.Status();
                SelectLogcat();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Priority()
        {
            _colorify.Clear();

            try
            {
                Section.Header("LOGCAT", "PRIORITY");

                Section.SelectedPackageName();

                _config.personal.logcat.priority = Selector.Start(Selector.Priority, "v");

                Menu.Status();
                SelectLogcat();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Show()
        {
            if (!String.IsNullOrEmpty(_config.personal.adb.wifiIpAddress))
            {
                _colorify.Clear();

                try
                {
                    _colorify.BlankLines();
                    if (CmdDevices())
                    {
                        string pid = "";
                        if (!String.IsNullOrEmpty(_config.personal.logcat.packageName))
                        {
                            pid = CmdGetPid(_config.personal.logcat.packageName);
                            if (String.IsNullOrEmpty(pid))
                            {
                                Message.Alert($" There is no app running with {_config.personal.logcat.packageName} package name.");
                            }
                        }
                        CmdLogcat(_config.personal.adb.deviceName, _config.personal.logcat.priority, pid);
                    }
                    else
                    {
                        Message.Alert(" No device/emulators found");
                    }

                    Menu.Start();
                }
                catch (Exception Ex)
                {
                    Exceptions.General(Ex);
                }
            }
        }
    }
}