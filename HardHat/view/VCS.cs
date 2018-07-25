using System;
using System.Collections.Generic;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using static Colorify.Colors;

namespace HardHat
{
    public static class Vcs
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "vd", status = false, action = Vcs.Discard });
            opts.Add(new Option { opt = "vp", status = false, action = Vcs.Pull });
            opts.Add(new Option { opt = "vr", status = false, action = Vcs.Reset });
            opts.Add(new Option { opt = "vd+p", status = false, action = Vcs.DiscardPull });
            opts.Add(new Option { opt = "vr+p", status = false, action = Vcs.ResetPull });
            opts.Add(new Option { opt = "vo", status = false, action = Vcs.Original });
        }

        public static void Status(string dirPath)
        {
            _config.personal.menu.currentBranch = "";
            if (!String.IsNullOrEmpty(_config.personal.selected.project))
            {
                string bnc = Git.CmdBranch(dirPath);
                if (!String.IsNullOrEmpty(bnc))
                {
                    _config.personal.menu.currentBranch = $"git://{Git.CmdBranch(dirPath)}";
                }
            }
            Options.IsValid("v", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("vd", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("vp", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("vr", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("vd+p", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("vr+p", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.IsValid("vo", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
        }

        public static void Start()
        {
            if (Options.IsValid("v"))
            {
                _colorify.WriteLine($" [V] VCS", txtMuted);
            }
            else
            {
                _colorify.Write($" [V] VCS: ", txtMuted);
                _colorify.WriteLine($"{_config.personal.menu.currentBranch}");
            }
            _colorify.Write($"{"   [P] Pull",-17}", txtStatus(Options.IsValid("vp")));
            _colorify.Write($"{"[D] Discard",-17}", txtStatus(Options.IsValid("vd")));
            _colorify.Write($"{"[R] Reset",-17}", txtStatus(Options.IsValid("vr")));
            _colorify.WriteLine($"{"[O] Original",-17}", txtStatus(Options.IsValid("vo")));
            _colorify.BlankLines();
        }

        public static void Discard()
        {
            Vcs.Actions(true, false, false, false);
        }

        public static void Pull()
        {
            Vcs.Actions(false, true, false, false);
        }

        public static void Reset()
        {
            Vcs.Actions(false, false, true, false);
        }

        public static void DiscardPull()
        {
            Vcs.Actions(true, true, false, false);
        }

        public static void ResetPull()
        {
            Vcs.Actions(false, true, true, false);
        }

        public static void Original()
        {
            Vcs.Actions(true, true, true, true);
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