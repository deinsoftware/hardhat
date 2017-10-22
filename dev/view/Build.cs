using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    public partial class Build {
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Build()
        {
            _c = Program.config;
            _cp = Program.config.personal;
        }
        
        public static void Select() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION");
                Section.SelectedProject();
                
                if (!_cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $"{" [D] Dimension:"    , -25}".txtPrimary();   $"{_cp.gdl.dmn}".txtDefault(ct.WriteLine);
                string b_flv = "";
                switch (_cp.gdl.flv?.ToLower())
                {
                    case "a":
                        b_flv = "Alfa";
                        break;
                    case "b":
                        b_flv = "Beta";
                        break;
                    case "s":
                        b_flv = "Stag";
                        break;
                    case "p":
                        b_flv = "Prod";
                        break;
                    case "d":
                        b_flv = "Desk";
                        break;
                }
                $"{" [F] Flavor:"       , -25}".txtPrimary();   $"{b_flv}".txtDefault(ct.WriteLine);
                string b_mde = "";
                switch (_cp.gdl.mde?.ToLower())
                {
                    case "d":
                        b_mde = "Debug";
                        break;
                    case "r":
                        b_mde = "Release";
                        break;
                }
                $"{" [M] Mode:"         , -25}".txtPrimary();   $"{b_mde}".txtDefault(ct.WriteLine);

                $"{"[EMPTY] Exit", 82}".txtDanger(ct.WriteLine);

                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Make your choice:", -25}".txtInfo();
                string opt = Console.ReadLine();

                if(String.IsNullOrEmpty(opt?.ToLower()))
                {
                    Menu.Start();
                } else {
                    Menu.Route($"b>{opt?.ToLower()}", "b");
                }
                Message.Error();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Dimension() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION > DIMENSION");
                Section.SelectedProject();

                if (!_cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" Write a project dimension:".txtPrimary(ct.WriteLine);
                $" EMPTY".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_dmn = Console.ReadLine();
                if (!String.IsNullOrEmpty(opt_dmn))
                {
                    _cp.gdl.dmn = $"{opt_dmn}";
                } else {
                    _cp.gdl.dmn = $"";
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Flavor() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION > FLAVOR");
                Section.SelectedProject();

                if (!_cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" {"A", 2}] Alfa".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                $" {"B", 2}] Beta".txtPrimary(ct.WriteLine);
                $" {"S", 2}] Stag".txtPrimary(ct.WriteLine);
                $" {"P", 2}] Prod".txtPrimary(ct.WriteLine);
                $" {"D", 2}] Desk".txtPrimary(ct.WriteLine);
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_flv = Console.ReadLine();
                opt_flv = opt_flv.ToLower();
                
                switch (opt_flv)
                {
                    case "a":
                    case "b":
                    case "s":
                    case "p":
                    case "d":
                        _cp.gdl.flv = opt_flv;
                        break;
                    case "":
                        _cp.gdl.flv = "a";
                        break;
                    default:
                        Message.Error();
                        break;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Mode() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION > MODE");
                Section.SelectedProject();

                if (!_cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{_cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $" {"D", 2}] Debug".txtPrimary(); $" (Default)".txtInfo(ct.WriteLine);
                $" {"R", 2}] Release".txtPrimary(ct.WriteLine);
                $"".fmNewLine();
                $"{"[EMPTY] Default", 82}".txtInfo(ct.WriteLine);
                
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();
            
                $"{" Make your choice: ", -25}".txtInfo();
                string opt_mde = Console.ReadLine();
                opt_mde = opt_mde.ToLower();
                
                switch (opt_mde)
                {
                    case "d":
                    case "r":
                        _cp.gdl.mde = opt_mde;
                        break;
                    case "":
                        _cp.gdl.mde = "d";
                        break;
                    default:
                        Message.Error();
                        break;
                }

                Menu.Status();
                Select();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Gradle() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Vpn.Verification();

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj); 
                CmdGradle(dirPath, _cp.mnu.b_cnf);

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Clean() {
            Colorify.Default();
            Console.Clear();

            try
            {
                Vpn.Verification();

                string dirPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj); 
                CmdClean(dirPath);

                Menu.Start();
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void Properties()
        {
            Colorify.Default();
            Console.Clear();

            try
            {
                Section.Header("BUILD CONFIGURATION > PROPERTIES");
                Section.SelectedProject();

                string sourcePath = Paths.Combine(Env.Get("ANDROID_PROPERTIES"), _c.path.bsn);
                string destinationPath = Paths.Combine(_c.path.dir, _c.path.bsn, _c.path.prj, _cp.spr, _c.android.prj); 
                
                $"".fmNewLine();
                List<string> filter = new List<string>() { 
                    ".properties"
                };

                $" --> Copying...".txtInfo(ct.WriteLine);
                $"".fmNewLine();
                $"{" From:", -8}".txtMuted(); $"{sourcePath}".txtDefault(ct.WriteLine);
                $"{" To:"  , -8}".txtMuted(); $"{destinationPath}".txtDefault(ct.WriteLine);
                Paths.CopyAll(sourcePath, destinationPath, true, true, filter);     
            
                $"".fmNewLine();
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $" Press [Any] key to continue...".txtInfo();
                Console.ReadKey();

                Menu.Start();
            }
            catch (Exception Ex)
            {
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}