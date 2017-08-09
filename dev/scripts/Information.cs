using System;
using System.Reflection;
using System.Runtime.InteropServices;
using dein.tools;
using static dein.tools.Machine;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    class Information {
        public static void Versions() {
            Colorify.Default();
            Console.Clear();

            $"=".bgInfo(ct.Repeat);
            $" COMMANDS".bgInfo(ct.PadLeft);
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();
            
            $" Required".txtInfo(ct.WriteLine);
            $"{" Gradle", -25}".txtPrimary();       Version.CmdGradle();
            $"{" Gulp", -25}".txtPrimary();         Version.CmdGulp();
            $"{" Java", -25}".txtPrimary();         Version.CmdJava();
            $"{" Node", -25}".txtPrimary();         Version.CmdNode();
            $"{" NPM", -25}".txtPrimary();          Version.CmdNPM();

            $"".fmNewLine();
            $" Optional".txtInfo(ct.WriteLine);
            $"{" Cordova", -25}".txtPrimary();      Version.CmdCordova();
            $"{" GIT", -25}".txtPrimary();          Version.CmdGit();
            $"{" NativeScript", -25}".txtPrimary(); Version.CmdNativescript();
            $"{" TypeScript", -25}".txtPrimary();   Version.CmdTypescript();

            $"".fmNewLine();
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            $" Press [Any] key to continue...".txtInfo();
            Console.ReadKey();

            Menu.Start();
        }

        public static void Environment() {
            Colorify.Default();
            Console.Clear();

            var c =  Program.config;

            $"=".bgInfo(ct.Repeat);
            $" ENVIRONMENT VARIABLES".bgInfo(ct.PadLeft);
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();
            
            string android = "ANDROID_HOME";
            $"{$" {android}:", -25}".txtPrimary();
            Env.Status(android);

            string ndk = "ANDROID_NDK_HOME";
            $"{$" {ndk}:", -25}".txtPrimary();
            Env.Status(ndk);

            string abt = "ANDROID_BT_VERSION";
            $"{$" {abt}:", -25}".txtPrimary();
            Env.Status(abt);

            string ast = "ANDROID_TEMPLATE";
            $"{$" {ast}:", -25}".txtPrimary();
            Env.Status(ast);

            string java = "JAVA_HOME";
            $"{$" {java}:", -25}".txtPrimary();
            Env.Status(java);

            string git = "GIT_HOME";
            $"{$" {git}:", -25}".txtPrimary();
            Env.Status(git);

            string gradle = "GRADLE_HOME";
            $"{$" {gradle}:", -25}".txtPrimary();
            Env.Status(gradle);

            string gulp = "GULP_PROJECT";
            $"{$" {gulp}:", -25}".txtPrimary();
            Env.Status(gulp);

            string vpn = "VPN_HOME";
            $"{$" {vpn}:", -25}".txtPrimary();
            Env.Status(vpn);

            $"".fmNewLine();
            $"=".bgInfo(ct.Repeat);
            $"".fmNewLine();

            $" Press [Any] key to continue...".txtInfo();
            Console.ReadKey();

            Menu.Start();
        }
    }
}