using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using dein.tools;
using static dein.tools.Machine;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    public static class Information {
        public static void Versions() {
            Colorify.Default();
            Console.Clear();

            Section.Header("COMMANDS");
            
            $" Required".txtInfo(ct.WriteLine);
            $"{" Gradle",           -25}".txtPrimary();           Version.CmdGradle();
            $"{" Gulp",             -25}".txtPrimary();             Version.CmdGulp();
            $"{" Java",             -25}".txtPrimary();             Version.CmdJava();
            $"{" Node",             -25}".txtPrimary();             Version.CmdNode();
            $"{" NPM",              -25}".txtPrimary();              Version.CmdNPM();

            $"".fmNewLine();
            $" Optional".txtInfo(ct.WriteLine);
            $"{" Cordova",          -25}".txtPrimary();          Version.CmdCordova();
            $"{" GIT",              -25}".txtPrimary();              Version.CmdGit();
            $"{" NativeScript",     -25}".txtPrimary();     Version.CmdNativescript();
            $"{" TypeScript",       -25}".txtPrimary();       Version.CmdTypescript();
            $"{" SonarLint",        -25}".txtPrimary();        Version.CmdSonarLint();
            $"{" SonarScanner",     -25}".txtPrimary();     Version.CmdSonarScanner();
            
            Section.HorizontalRule();
            Section.Pause();

            Menu.Start();
        }

        public static void Environment() {
            Colorify.Default();
            Console.Clear();

            Variables.Upgrade();
            Variables.Update();
            
            Section.Header("ENVIRONMENT VARIABLES");

            foreach (var v in Variables.list)
            {
                $"{$" {v.nme}:", -25}".txtPrimary();
                if (v.stt) {
                    $"{v.vlu.Slash()}".txtDefault(ct.WriteLine);
                } else {
                    $"is not defined".txtWarning(ct.WriteLine);
                }
            }

            Section.HorizontalRule();
            Section.Pause();

            Menu.Start();
        }
    }
}