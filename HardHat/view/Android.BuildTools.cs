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

    public static partial class BuildTools
    {
        public static void Values()
        {
            _colorify.Clear();

            try
            {
                Section.Header("FILE", "INFORMATION");
                Section.SelectedFile();

                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project, _config.android.projectPath, _config.android.buildPath, _config.personal.selected.path, _config.personal.selected.file);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Dump Badging...", txtInfo);
                CmdInformation(dirPath);

                _colorify.BlankLines();
                _colorify.WriteLine($" --> Signer Verify...", txtInfo);
                CmdSignerVerify(dirPath);

                if ((OS.IsWin() && Variables.Valid("sh")) || OS.IsMac())
                {
                    Response result = CmdSha(dirPath);
                    if (result.code == 0)
                    {
                        _colorify.BlankLines();
                        _colorify.WriteLine($" --> File Hash...", txtInfo);

                        _colorify.BlankLines();
                        _colorify.Write($" SHA256: ", txtMuted);
                        _colorify.WriteLine($"{result.stdout}");
                    }
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

        public static void Upgrade()
        {
            try
            {
                string currentVersion = Variables.Value("ab");
                string lastVersion = "";
                string dirPath = _path.Combine(Variables.Value("ah"), "build-tools");

                if (_fileSystem.DirectoryExists(dirPath))
                {
                    string dir = Directory.EnumerateDirectories(dirPath).OrderByDescending(name => name).Take(1).FirstOrDefault();
                    string d = dir;
                    lastVersion = _path.GetFileName(d);
                    if (currentVersion != lastVersion)
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.Append($"There is a new Android Build Tools version installed.");
                        msg.Append(Environment.NewLine);
                        msg.Append($" Please verify your Environment Variables and change ANDROID_BT_VERSION from {currentVersion} to {lastVersion}.");
                        Message.Alert(msg.ToString());
                    }
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}