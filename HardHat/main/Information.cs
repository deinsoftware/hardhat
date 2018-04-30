using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using Colorify;
using static Colorify.Colors;
using dein.tools;
using static HardHat.Program;

namespace HardHat {

    public static class Information {

        public static void List(ref List<Option> opts) {
            opts.Add(new Option{opt="i"   , stt=true , act=Information.Versions             });
            opts.Add(new Option{opt="e"   , stt=true , act=Information.Environment          });
        }

        public static void Versions() {
            _colorify.Clear();

            Section.Header("COMMANDS");
            
            _colorify.WriteLine(" Required", txtInfo);
            _colorify.Write($"{" Gradle",           -25}", txtPrimary);           Version.CmdGradle();
            _colorify.Write($"{" Gulp",             -25}", txtPrimary);             Version.CmdGulp();
            _colorify.Write($"{" Java",             -25}", txtPrimary);             Version.CmdJava();
            _colorify.Write($"{" Node",             -25}", txtPrimary);             Version.CmdNode();
            _colorify.Write($"{" NPM",              -25}", txtPrimary);              Version.CmdNPM();

            _colorify.BlankLines();
            _colorify.WriteLine(" Optional", txtInfo);
            _colorify.Write($"{" Cordova",          -25}", txtPrimary);          Version.CmdCordova();
            _colorify.Write($"{" GIT",              -25}", txtPrimary);              Version.CmdGit();
            _colorify.Write($"{" NativeScript",     -25}", txtPrimary);     Version.CmdNativescript();
            _colorify.Write($"{" TypeScript",       -25}", txtPrimary);       Version.CmdTypescript();
            _colorify.Write($"{" SonarScanner",     -25}", txtPrimary);     Version.CmdSonarScanner();
            
            Section.HorizontalRule();
            Section.Pause();

            Menu.Start();
        }

        public static void Environment() {
            Variables.Upgrade();
            Variables.Update();

            _colorify.Clear();

            Section.Header("ENVIRONMENT VARIABLES");

            foreach (var v in Variables.list)
            {
                _colorify.Write($"{$" {v.nme}:", -21}", txtPrimary);
                if (v.stt) {
                    _colorify.WriteLine($"{v.vlu}");
                } else {
                    _colorify.WriteLine("is not defined", txtWarning);
                }
            }

            Section.HorizontalRule();
            Section.Pause();

            Menu.Start();
        }
    }
}