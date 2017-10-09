using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {
    static partial class Build {
        public static void Select() {
            Colorify.Default();
            Console.Clear();

            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" BUILD CONFIGURATION".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
                }

                $"".fmNewLine();
                $"{" [D] Dimension:"    , -25}".txtPrimary();   $"{cp.gdl.dmn}".txtDefault(ct.WriteLine);
                string b_flv = "";
                switch (cp.gdl.flv?.ToLower())
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
                switch (cp.gdl.mde?.ToLower())
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
                cp.mnu.sel = $"b>{opt.ToLower()}";

                switch (opt.ToLower())
                {
                    case "d":
                        Dimension();
                        break;
                    case "f":
                        Flavor();
                        break;
                    case "m":
                        Mode();
                        break;
                    case "":
                        Menu.Start();
                        break;
                    default:
                        cp.mnu.sel = "b";
                        break;
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

            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" BUILD CONFIGURATION > DIMENSION".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
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
                    cp.gdl.dmn = $"{opt_dmn}";
                } else {
                    cp.gdl.dmn = $"";
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

            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" BUILD CONFIGURATION > FLAVOR".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
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
                        cp.gdl.flv = opt_flv;
                        break;
                    case "":
                        cp.gdl.flv = "a";
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

            var cp =  Program.config.personal;
            try
            {
                $"=".bgInfo(ct.Repeat);
                $" BUILD CONFIGURATION > MODE".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

                if (!cp.mnu.b_sel)
                {
                    $"{" Current Configuration:", -25}".txtMuted();
                    $"{cp.mnu.b_cnf}".txtDefault(ct.WriteLine);
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
                        cp.gdl.mde = opt_mde;
                        break;
                    case "":
                        cp.gdl.mde = "d";
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

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                Vpn.Verification();

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj); 
                CmdGradle(dirPath, cp.mnu.b_cnf);

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

            var c =  Program.config;
            var cp =  Program.config.personal;
            try
            {
                Vpn.Verification();

                string dirPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj); 
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

            var c = Program.config;
            var cp = Program.config.personal;
            try
            {
                string sourcePath = Paths.Combine(dein.tools.Env.Get("ANDROID_TEMPLATE"));
                string destinationPath = Paths.Combine(c.path.dir, c.path.bsn, c.path.prj, cp.spr, c.android.prj); 

                $"=".bgInfo(ct.Repeat);
                $" BUILD CONFIGURATION > PROPERTIES".bgInfo(ct.PadLeft);
                $"=".bgInfo(ct.Repeat);
                $"".fmNewLine();

                $"{" Selected Project:", -25}".txtMuted();
                $"{cp.spr}".txtDefault(ct.WriteLine);

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