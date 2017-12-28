using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using ToolBox.Validations;
using dein.tools;
using static HardHat.Program;
using static dein.tools.Paths;
using Colorify;
using static Colorify.Colors;

namespace HardHat {

    public static class Project {


        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="p"   , stt=true , act=Project.Select                   });
            opts.Add(new Option{opt="pf"  , stt=false, act=Project.SelectFile               });
            opts.Add(new Option{opt="pi"  , stt=false, act=Adb.Install                      });
            opts.Add(new Option{opt="pd"  , stt=false, act=Project.Duplicate                });
            opts.Add(new Option{opt="pp"  , stt=false, act=Project.FilePath                 });
            opts.Add(new Option{opt="ps"  , stt=false, act=BuildTools.SignerVerify          });
            opts.Add(new Option{opt="pv"  , stt=false, act=BuildTools.Information           });
        }

        public static void Status(string dirPath){
            if (!Directory.Exists(dirPath))
            {
                _config.personal.spr = "";
            }
            Options.Valid("p" , true);
            string filePath = _path.Combine(dirPath, _config.android.prj, _config.android.bld, _config.personal.sfl ?? "");
            if (!File.Exists(filePath))
            {
                _config.personal.sfl = "";
            }
            Options.Valid("pf", !Strings.SomeNullOrEmpty(_config.personal.spr));
            Options.Valid("pi", !Strings.SomeNullOrEmpty(_config.personal.spr, _config.personal.sfl));
            Options.Valid("pd", !Strings.SomeNullOrEmpty(_config.personal.spr, _config.personal.sfl));
            Options.Valid("pp", !Strings.SomeNullOrEmpty(_config.personal.spr, _config.personal.sfl));
            Options.Valid("ps", !Strings.SomeNullOrEmpty(_config.personal.spr, _config.personal.sfl));
            Options.Valid("pv", !Strings.SomeNullOrEmpty(_config.personal.spr, _config.personal.sfl));
        }

        public static void Start() {
            if (String.IsNullOrEmpty(_config.personal.spr))
            {
                _colorify.WriteLine($" [P] Select Project", txtPrimary);
            } else {
                _colorify.Write($" [P] Selected Project: ", txtPrimary);
                _colorify.WriteLine($"{_config.personal.spr}");
            }
            
            if (String.IsNullOrEmpty(_config.personal.spr))
            {
                _colorify.WriteLine($"   [F] Select File", txtStatus(Options.Valid("pf")));
            } else {
                _colorify.Write($"   [F] Selected File:  ", txtPrimary);
                _colorify.WriteLine($"{_config.personal.sfl}");
            }

            _colorify.Write($"{"   [I] Install" , -17}", txtStatus(Options.Valid("pi")));
            _colorify.Write($"{"[D] Duplicate"  , -17}", txtStatus(Options.Valid("pd")));
            _colorify.Write($"{"[P] Path"       , -17}", txtStatus(Options.Valid("pp")));
            _colorify.Write($"{"[S] Signer"     , -17}", txtStatus(Options.Valid("ps")));
            _colorify.WriteLine($"{"[V] Values" , -17}", txtStatus(Options.Valid("pv")));

            _colorify.BlankLines();
        }

        public static void Select() {
            _colorify.Clear();

            try
            {
                Section.Header("SELECT PROJECT");
                
                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj);
                dirPath.Exists("Please review your configuration file.");
                List<string> dirs = dirPath.Directories(_config.path.flt, "projects");

                if (dirs.Count < 1) {
                    _config.personal.spr = "";
                } else {
                    var i = 1;
                    foreach (var dir in dirs)
                    {
                        string d = dir;
                        _colorify.WriteLine($" {i, 2}] {_path.GetDirectoryName(d)}", txtPrimary);
                        i++;
                    }
                }

                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);
                
                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), dirs.Count);

                    var sel = dirs[Convert.ToInt32(opt) - 1];
                    _config.personal.spr = _path.GetDirectoryName(sel);
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void SelectFile() {
            _colorify.Clear();

            try
            {
                Section.Header("SELECT FILE");
                
                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld);
                dirPath.Exists("Please review your configuration file or make a build first.");
                List<string> files = dirPath.Files($"*{_config.android.ext}", "Please make a build first.");
                
                if (files.Count < 1)
                {
                    _config.personal.sfl = "";
                } else {
                    var i = 1;
                    foreach (var file in files)
                    {
                        string f = file;
                        _colorify.WriteLine($" {i, 2}] {_path.GetFileName(f)}", txtPrimary);
                        i++;
                    }
                }
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), files.Count);
                    var sel = files[Convert.ToInt32(opt) - 1];
                    _config.personal.sfl = _path.GetFileName(sel);
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        public static void Duplicate() {
            _colorify.Clear();

            try
            {
                Section.Header("DUPLICATE FILE");
                Section.SelectedFile();

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld); 

                _colorify.BlankLines();
                _colorify.WriteLine($" Write a new name, without include his extension.", txtPrimary);
                
                _colorify.BlankLines();
                _colorify.WriteLine($"{"[EMPTY] Cancel", 82}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice: ", -25}", txtInfo);
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    System.IO.File.Copy(_path.Combine(dirPath, _config.personal.sfl), _path.Combine(dirPath, $"{opt}{_config.android.ext}"));
                    _config.personal.sfl = $"{opt}{_config.android.ext}";
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void FilePath() {
            _colorify.Clear();

            try
            {
                Section.Header("FILE PATH");
                
                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld); 

                _colorify.Write($"{" Path:", -10}", txtMuted);
                _colorify.WriteLine($"{dirPath}");

                _colorify.Write($"{" File:", -10}", txtMuted);
                _colorify.WriteLine($"{_config.personal.sfl}");

                _colorify.BlankLines();
                _colorify.Write($"{" [P] Copy Path", -34}", txtInfo);
                _colorify.Write($"{"[F] Copy Full Path", -34}", txtInfo);
                _colorify.WriteLine($"{"[EMPTY] Cancel", -17}", txtDanger);

                Section.HorizontalRule();

                _colorify.Write($"{" Make your choice:", -25}", txtInfo);
                string opt = Console.ReadLine();

                switch (opt?.ToLower())
                {
                    case "p":
                        Clipboard.Copy(dirPath);
                        break;
                    case "f":
                        Clipboard.Copy(_path.Combine(dirPath, _config.personal.sfl));
                        break;
                    case "":
                        //Cancel
                        break;
                    default:
                        Message.Error();
                        break;
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}