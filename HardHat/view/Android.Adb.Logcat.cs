using System;
using System.Collections.Generic;
using static Colorify.Colors;
using dein.tools;
using static HardHat.Program;

namespace HardHat
{

    public static partial class Adb
    {
        public static void LogcatList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "al", status = true, action = Adb.Logcat });
            opts.Add(new Option { opt = "al>p", status = true, action = Adb.Priority });
            opts.Add(new Option { opt = "al>f", status = true, action = Adb.Filter });
            opts.Add(new Option { opt = "al>s", status = true, action = Adb.All });
            opts.Add(new Option { opt = "al>a", status = true, action = Adb.Application });
        }

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
                string logcatPriority = Selector.Name(Selector.Priority, _config.personal.logcat.priority);
                _colorify.Write($"{" [P] Priority:",-25}", txtPrimary); _colorify.WriteLine($"{logcatPriority}");
                _colorify.Write($"{" [F] Filter:",-25}", txtPrimary); _colorify.WriteLine($"{_config.personal.logcat.filter}");

                _colorify.BlankLines();
                _colorify.Write($"{" [S] Show All",-17}", txtPrimary);
                _colorify.Write($"{" [A] Application",-51}", txtPrimary);
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

        public static void Filter()
        {
            _colorify.Clear();

            try
            {
                Section.Header("LOGCAT", "FILTER");

                Section.SelectedFile();
                Section.SelectedPackageName();

                _colorify.BlankLines();
                _colorify.WriteLine($" Write filter.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Remove",82}", txtWarning);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                _config.personal.logcat.filter = $"{opt}";

                Menu.Status();
                SelectLogcat();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void All()
        {
            Show();
        }

        public static void Application()
        {
            Show(_config.personal.selected.packageName);
        }

        public static void Show(string packageName = "")
        {
            _colorify.Clear();

            try
            {
                _colorify.BlankLines();
                if (CmdDevices())
                {
                    string pid = "";
                    if (!String.IsNullOrEmpty(packageName))
                    {
                        pid = CmdGetPid(packageName);
                        if (String.IsNullOrEmpty(pid))
                        {
                            Message.Alert($" There is no app running with {packageName} package name.");
                        }
                    }
                    CmdLogcat(_config.personal.adb.deviceName, _config.personal.logcat, pid);
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