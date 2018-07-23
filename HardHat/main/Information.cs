using System;
using System.Collections.Generic;
using dein.tools;
using static Colorify.Colors;
using static HardHat.Program;

namespace HardHat
{

    public static class Information
    {

        public static void List(ref List<Option> opts)
        {
            opts.Add(new Option { opt = "i", status = true, action = Information.Versions });
            opts.Add(new Option { opt = "i>r", status = true, action = Information.Readme });
            opts.Add(new Option { opt = "i>c", status = true, action = Information.Changelog });
            opts.Add(new Option { opt = "e", status = true, action = Information.Environment });
        }

        public static void Versions()
        {
            _colorify.Clear();

            Section.Header("INFORMATION");

            _colorify.WriteLine(" Required", txtInfo);
            _colorify.Write($"{" Gradle",-25}", txtPrimary); Version.CmdGradle();
            _colorify.Write($"{" Gulp",-25}", txtPrimary); Version.CmdGulp();
            _colorify.Write($"{" Java",-25}", txtPrimary); Version.CmdJava();
            _colorify.Write($"{" Node",-25}", txtPrimary); Version.CmdNode();
            _colorify.Write($"{" NPM",-25}", txtPrimary); Version.CmdNPM();

            _colorify.BlankLines();
            _colorify.WriteLine(" Optional", txtInfo);
            _colorify.Write($"{" Cordova",-25}", txtPrimary); Version.CmdCordova();
            _colorify.Write($"{" GIT",-25}", txtPrimary); Version.CmdGit();
            _colorify.Write($"{" NativeScript",-25}", txtPrimary); Version.CmdNativescript();
            _colorify.Write($"{" TypeScript",-25}", txtPrimary); Version.CmdTypescript();
            _colorify.Write($"{" SonarScanner",-25}", txtPrimary); Version.CmdSonarScanner();

            _colorify.BlankLines();
            _colorify.Write($"{" [R] Readme",-17}", txtInfo);
            _colorify.Write($"{"[C] Changelog",-51}", txtInfo);
            _colorify.WriteLine($"{"[EMPTY] Cancel",-17}", txtDanger);

            Section.HorizontalRule();

            _colorify.Write($"{" Make your choice:",-25}", txtInfo);
            string opt = Console.ReadLine()?.ToLower();

            if (String.IsNullOrEmpty(opt))
            {
                Menu.Start();
            }
            else
            {
                Menu.Route($"i>{opt}", "i");
            }
            Message.Error();
        }

        public static void Environment()
        {
            Variables.Upgrade();
            Variables.Update();

            _colorify.Clear();

            Section.Header("ENVIRONMENT VARIABLES");

            foreach (var v in Variables.list)
            {
                _colorify.Write($"{$" {v.name}:",-21}", txtPrimary);
                if (v.status)
                {
                    _colorify.WriteLine($"{v.value}");
                }
                else
                {
                    _colorify.WriteLine("is not defined", txtWarning);
                }
            }

            Section.HorizontalRule();
            Section.Pause();

            Menu.Start();
        }

        private static string SiteUrl(string page)
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = _config.project.url;
            uriBuilder.Path = $"{_config.project.user}/{_config.project.name}/{_config.project.content}/{page}";
            return uriBuilder.ToString();
        }

        public static void Readme()
        {
            _colorify.Clear();

            try
            {

                string url = SiteUrl(_config.project.readme);
                Browser.CmdOpen(url);
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static void Changelog()
        {
            _colorify.Clear();

            try
            {
                string url = SiteUrl(_config.project.changelog);
                Browser.CmdOpen(url);
                Menu.Start();
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }
    }
}