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

using ct = dein.tools.Colorify.Type;

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
                $" [P] Select Project".txtPrimary(ct.WriteLine);
            } else {
                $" [P] Selected Project: ".txtPrimary();
                $"{_config.personal.spr}".txtDefault(ct.WriteLine);
            }
            
            if (String.IsNullOrEmpty(_config.personal.spr))
            {
                $"   [F] Select File".txtStatus(ct.WriteLine,   Options.Valid("pf"));
            } else {
                $"   [F] Selected File:  ".txtPrimary();
                $"{_config.personal.sfl}".txtDefault(ct.WriteLine);
            }

            $"{"   [I] Install" , -17}".txtStatus(ct.Write,     Options.Valid("pi"));
            $"{"[D] Duplicate"  , -17}".txtStatus(ct.Write,     Options.Valid("pd"));
            $"{"[P] Path"       , -17}".txtStatus(ct.Write,     Options.Valid("pp"));
            $"{"[S] Signer"     , -17}".txtStatus(ct.Write,     Options.Valid("ps"));
            $"{"[V] Values"     , -17}".txtStatus(ct.WriteLine, Options.Valid("pv"));

            $"".fmNewLine();
        }

        public static void Select() {
            Colorify.Default();

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
                        $" {i, 2}] {d.Substring(d.LastIndexOf("/") + 1)}".txtPrimary(ct.WriteLine);
                        i++;
                    }
                }

                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);
                
                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), dirs.Count);

                    var sel = dirs[Convert.ToInt32(opt) - 1];
                    _config.personal.spr = sel.Substring(sel.LastIndexOf("/") + 1);
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static void SelectFile() {
            Colorify.Default();

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
                        $" {i, 2}] {f.Substring(f.LastIndexOf("/") + 1)}".txtPrimary(ct.WriteLine);
                        i++;
                    }
                }
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();
                
                if (!String.IsNullOrEmpty(opt))
                {
                    Number.IsOnRange(1, Convert.ToInt32(opt), files.Count);
                    var sel = files[Convert.ToInt32(opt) - 1];
                    _config.personal.sfl = sel.Substring(sel.LastIndexOf("/") + 1);
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
        public static void Duplicate() {
            Colorify.Default();

            try
            {
                Section.Header("DUPLICATE FILE");
                Section.SelectedFile();

                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld); 

                $"".fmNewLine();
                $" Write a new name, without include his extension.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice: ", -25}".txtInfo();
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
            Colorify.Default();

            try
            {
                Section.Header("FILE PATH");
                
                string dirPath = _path.Combine(_config.path.dir, _config.path.bsn, _config.path.prj, _config.personal.spr, _config.android.prj, _config.android.bld); 

                $"{" Path:", -10}".txtMuted();
                $"{dirPath}".txtDefault(ct.WriteLine);

                $"{" File:", -10}".txtMuted();
                $"{_config.personal.sfl}".txtDefault(ct.WriteLine);

                $"".fmNewLine();
                $"{" [P] Copy Path", -34}".txtInfo();
                $"{"[F] Copy Full Path", -34}".txtInfo();
                $"{"[EMPTY] Cancel", -17}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice:", -25}".txtInfo();
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