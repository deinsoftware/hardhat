using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;
using static dein.tools.Paths;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public class Project {

        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Project()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }

        public static void Select() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("SELECT PROJECT");
                
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj);
                dirPath.Exists("Please review your configuration file.");
                List<string> dirs = dirPath.Directories(_c.path.flt, "projects");

                if (dirs.Count < 1) {
                    _cp.spr = "";
                } else {
                    var i = 1;
                    foreach (var dir in dirs)
                    {
                        string d = dir.Slash();
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
                    Validation.Range(opt, 1, dirs.Count);
                    
                    var sel = dirs[Convert.ToInt32(opt) - 1].Slash();
                    _cp.spr = sel.Substring(sel.LastIndexOf("/") + 1);
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void File() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("SELECT FILE");
                
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld);
                dirPath.Exists("Please review your configuration file or make a build first.");
                List<string> files = dirPath.Files($"*{_c.android.ext}", "Please make a build first.");
                
                if (files.Count < 1)
                {
                    _cp.sfl = "";
                } else {
                    var i = 1;
                    foreach (var file in files)
                    {
                        string f = file.Slash();
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
                    Validation.Range(opt, 1, files.Count);
                    var sel = files[Convert.ToInt32(opt) - 1].Slash();
                    _cp.sfl = sel.Substring(sel.LastIndexOf("/") + 1);
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
        public static void Duplicate() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("DUPLICATE FILE");
                Section.SelectedFile();

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld); 

                $"".fmNewLine();
                $" Write a new name, without include his extension.".txtPrimary(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Cancel", 82}".txtDanger(ct.WriteLine);

                Section.HorizontalRule();

                $"{" Make your choice: ", -25}".txtInfo();
                string opt = Console.ReadLine();

                if (!String.IsNullOrEmpty(opt))
                {
                    System.IO.File.Copy(Paths.Combine(dirPath, _cp.sfl), Paths.Combine(dirPath, $"{opt}{_c.android.ext}"));
                    _cp.sfl = $"{opt}{_c.android.ext}";
                }

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical (
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void FilePath() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("FILE PATH");
                
                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj, _c.android.bld); 

                $"{" Path:", -10}".txtMuted();
                $"{dirPath}".txtDefault(ct.WriteLine);

                $"{" File:", -10}".txtMuted();
                $"{_cp.sfl}".txtDefault(ct.WriteLine);

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
                        Clipboard.Copy(Paths.Combine(dirPath, _cp.sfl));
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}