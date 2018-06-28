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
            Options.Valid("v", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.Valid("vd", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.Valid("vp", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.Valid("vr", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.Valid("vd+p", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
            Options.Valid("vr+p", Variables.Valid("gh") && !Strings.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.menu.currentBranch));
        }

        public static void Start()
        {
            if (Options.Valid("v"))
            {
                _colorify.WriteLine($" [V] VCS", txtMuted);
            }
            else
            {
                _colorify.Write($" [V] VCS: ", txtMuted);
                _colorify.WriteLine($"{_config.personal.menu.currentBranch}");
            }
            _colorify.Write($"{"   [D] Discard",-34}", txtStatus(Options.Valid("vd")));
            _colorify.Write($"{"[P] Pull",-34}", txtStatus(Options.Valid("vp")));
            _colorify.WriteLine($"{"[R] Reset",-17}", txtStatus(Options.Valid("vr")));
            _colorify.BlankLines();
        }

        public static void Discard()
        {
            Vcs.Actions(true, false, false);
        }

        public static void Pull()
        {
            Vcs.Actions(false, true, false);
        }

        public static void Reset()
        {
            Vcs.Actions(false, false, true);
        }

        public static void DiscardPull()
        {
            Vcs.Actions(true, true, false);
        }

        public static void ResetPull()
        {
            Vcs.Actions(false, true, true);
        }

        public static void Actions(bool discard, bool pull, bool reset)
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