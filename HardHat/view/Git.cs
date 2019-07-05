using System;
using System.Collections.Generic;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using static Colorify.Colors;

namespace HardHat
{
    public static partial class Git
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "gd", status = false, action = Git.Discard });
            opts.Add(new Option { opt = "gp", status = false, action = Git.Pull });
            opts.Add(new Option { opt = "gr", status = false, action = Git.Reset });
            opts.Add(new Option { opt = "gd+p", status = false, action = Git.DiscardPull });
            opts.Add(new Option { opt = "gr+p", status = false, action = Git.ResetPull });
            opts.Add(new Option { opt = "go", status = false, action = Git.Original });
        }

        public static void Status(string dirPath)
        {
            _config.personal.menu.currentBranch = "";
            if (!String.IsNullOrEmpty(_config.personal.selected.project))
            {
                string bnc = Git.CmdBranch(dirPath);
                if (!String.IsNullOrEmpty(bnc))
                {
                    _config.personal.menu.currentBranch = $"{Git.CmdBranch(dirPath)}";
                }
            }
            Options.IsValid("g", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("gd", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("gp", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("gr", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("gd+p", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("gr+p", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("go", Variables.Valid("git") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
        }

        public static void Start()
        {
            if (Options.IsValid("g"))
            {
                _colorify.WriteLine($"{"[G] Git",-12}", txtMuted);
            }
            else
            {
                _colorify.Write($"{"[G] Git: ",-12}", txtMuted);
                _colorify.WriteLine($"{_config.personal.menu.currentBranch}");
            }
            _colorify.Write($"{"   [P] Pull",-17}", txtStatus(Options.IsValid("gp")));
            _colorify.Write($"{"[D] Discard",-17}", txtStatus(Options.IsValid("gd")));
            _colorify.Write($"{"[R] Reset",-17}", txtStatus(Options.IsValid("gr")));
            _colorify.WriteLine($"{"[O] Original",-17}", txtStatus(Options.IsValid("go")));
            _colorify.BlankLines();
        }

        public static void Discard()
        {
            Git.Actions(true, false, false, false);
        }

        public static void Pull()
        {
            Git.Actions(false, true, false, false);
        }

        public static void Reset()
        {
            Git.Actions(false, false, true, false);
        }

        public static void DiscardPull()
        {
            Git.Actions(true, true, false, false);
        }

        public static void ResetPull()
        {
            Git.Actions(false, true, true, false);
        }

        public static void Original()
        {
            Git.Actions(true, true, true, true);
        }

        public static void Actions(bool discard, bool pull, bool reset, bool confirm)
        {
            _colorify.Clear();

            try
            {
                Section.Header("GIT");
                Section.SelectedProject();

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);

                if (discard)
                {
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Discarding...", txtInfo);
                    Git.CmdDiscard(dirPath);
                }

                if (reset)
                {
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Reseting...", txtInfo);
                    Git.CmdReset(dirPath);
                }

                if (pull)
                {
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Updating...", txtInfo);
                    Git.CmdPull(dirPath);
                }

                if (confirm)
                {
                    _colorify.BlankLines();
                    _colorify.WriteLine($" --> Confirm update...", txtInfo);
                    Git.CmdPull(dirPath);
                }

                Section.HorizontalRule();
                Section.Pause();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}