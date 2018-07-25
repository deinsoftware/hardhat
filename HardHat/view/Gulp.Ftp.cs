using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Validation = ToolBox.Validations.Strings;
using Transform = ToolBox.Transform.Strings;
using dein.tools;
using static HardHat.Program;
using ToolBox.Validations;
using static Colorify.Colors;

namespace HardHat
{

    public static partial class Gulp
    {

        public static void FtpList(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "gf", status = false, action = Gulp.Ftp });
            opts.Add(new Option { opt = "g>fh", status = false, action = Gulp.Host });
            opts.Add(new Option { opt = "g>fp", status = false, action = Gulp.Port });
            opts.Add(new Option { opt = "g>fa", status = false, action = Gulp.AuthenticationFile });
            opts.Add(new Option { opt = "g>fk", status = false, action = Gulp.AuthenticationKey });
            opts.Add(new Option { opt = "g>fr", status = false, action = Gulp.RemotePath });
            opts.Add(new Option { opt = "g>fd", status = false, action = Gulp.Dimension });
        }

        public static void FtpStatus()
        {
            StringBuilder ftpConfiguration = new StringBuilder();
            ftpConfiguration.Append($"sftp://");
            if (!String.IsNullOrEmpty(_config.personal.ftpServer.host))
            {
                ftpConfiguration.Append($"sftp://{_config.personal.ftpServer.host}");
                if (_config.personal.ftpServer.port != 22)
                {
                    ftpConfiguration.Append($":{_config.personal.ftpServer.port}/");
                }
                else
                {
                    ftpConfiguration.Append($"/");
                }
                if (!String.IsNullOrEmpty(_config.personal.ftpServer.dimension))
                {
                    ftpConfiguration.Append($"{_config.personal.ftpServer.dimension}/");
                }
            }
            _config.personal.menu.ftpConfiguration = ftpConfiguration.ToString();
            _config.personal.menu.ftpValidation = !Validation.SomeNullOrEmpty(_config.personal.selected.project, _config.personal.ftpServer.host, _config.personal.menu.ftpConfiguration);

            Options.IsValid("gf", Variables.Valid("gp") && _config.personal.menu.ftpValidation);
            Options.IsValid("g>fh", Variables.Valid("gp"));
            Options.IsValid("g>fp", Variables.Valid("gp"));
            Options.IsValid("g>fa", Variables.Valid("gp"));
            Options.IsValid("g>fk", Variables.Valid("gp"));
            Options.IsValid("g>fr", Variables.Valid("gp"));
            Options.IsValid("g>fd", Variables.Valid("gp"));
        }

        public static void Ftp()
        {
            _colorify.Clear();
            try
            {
                string dirPath = _path.Combine(_config.path.development, _config.path.workspace, _config.path.project, _config.personal.selected.project);
                string selectedPath = _path.Combine(dirPath, _config.project.androidPath, _config.project.androidBuildPath, _config.personal.selected.path);
                _config.personal.ftpServer.resourcePath = _config.personal.selected.versionName;
                CmdFtp(
                    selectedPath,
                    _config.personal.ftpServer
                );
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Host()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "FTP", "HOST");

                _colorify.BlankLines();
                _colorify.WriteLine($" Host or IP address.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel",82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    _config.personal.ftpServer.host = opt;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Port()
        {
            _colorify.Clear();

            try
            {
                Section.Header("CONNECT DEVICE", "PORT");

                _colorify.WriteLine($" FTP port.", txtPrimary);
                _colorify.Write($" Between 0 and 65536.", txtPrimary); _colorify.Write($" 22 (Default)", txtInfo);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(0, Convert.ToInt32(opt), 65536);
                    _config.personal.ftpServer.port = Convert.ToInt32(opt);
                }
                else
                {
                    _config.personal.ftpServer.port = 22;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void AuthenticationFile()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "FTP", "AUTHENTICATION FILE PATH");

                _colorify.BlankLines();
                _colorify.WriteLine($" Relative path to Authentication File.", txtPrimary);
                _colorify.Write($" ../FTP/.ftppass (Default)", txtInfo);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    _config.personal.ftpServer.authenticationPath = $"{opt}";
                }
                else
                {
                    _config.personal.ftpServer.authenticationPath = "../FTP/.ftppass";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void AuthenticationKey()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "FTP", "AUTHENTICATION KEY");

                _colorify.BlankLines();
                _colorify.WriteLine($" Authentication Key name.", txtPrimary);
                _colorify.Write($" KeyMain (Default)", txtInfo);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    _config.personal.ftpServer.authenticationKey = $"{opt}";
                }
                else
                {
                    _config.personal.ftpServer.authenticationKey = "keyMain";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void RemotePath()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "FTP", "REMOTE PATH");

                _colorify.BlankLines();
                _colorify.WriteLine($" Remote path in FTP server.", txtPrimary);
                _colorify.Write($" / (Default)", txtInfo);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Default",82}", txtInfo);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();

                if (!String.IsNullOrEmpty(opt))
                {
                    _config.personal.ftpServer.remotePath = $"{opt}";
                }
                else
                {
                    _config.personal.ftpServer.remotePath = "/";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Dimension()
        {
            _colorify.Clear();

            try
            {
                Section.Header("GULP", "FTP", "DIMENSION");

                _colorify.BlankLines();
                _colorify.WriteLine($" Dimension inside remote path in FTP server.", txtPrimary);

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Remove",82}", txtWarning);

                Section.HorizontalRule();

                _colorify.Write($"{" Write your choice: ",-25}", txtInfo);
                string opt = Console.ReadLine().Trim();
                _config.personal.ftpServer.dimension = $"{opt}";


                Menu.Status();
                Select();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}