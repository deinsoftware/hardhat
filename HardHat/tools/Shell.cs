using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using System.IO;
using ToolBox.Platform;
using ToolBox.Transform;
using static HardHat.Program;
using Colorify;
using static Colorify.Colors;

namespace dein.tools
{
    public class Response {
        public int code { get; set; }
        public string stdout { get; set; }
        public string stderr { get; set; }
    }

    public enum Output {
        Hidden,
        Internal,
        External
    }

    public static class Shell
    {
        private static void Args (ref string fnm, ref string cmd, Output? output = Output.Hidden, string dir = "")
        {
            try
            {
                if (!String.IsNullOrEmpty(dir))
                {
                    dir.Exists("");
                }
                switch (OS.GetCurrent())
                {
                    case "win":
                        fnm = "cmd.exe";
                        if (!String.IsNullOrEmpty(dir))
                        {
                            dir = $" \"{dir}\"";
                        }
                        if (output == Output.External)
                        {
                            cmd = $"{Directory.GetCurrentDirectory()}/cmd.win.bat \"{cmd}\"{dir}";
                        }
                        cmd = $"/c \"{cmd}\"";
                        break;
                    case "mac":
                        fnm = "/bin/bash";
                        if (!String.IsNullOrEmpty(dir))
                        {
                            dir = $" '{dir}'";
                        }
                        if (output == Output.External)
                        {
                            cmd = $"sh {Directory.GetCurrentDirectory()}/cmd.mac.sh '{cmd}'{dir}";
                        }
                        cmd = $"-c \"{cmd}\"";
                        break;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
        }

        public static Response Term (this string cmd, Output? output = Output.Hidden, string dir = ""){
            var result = new Response();
            var stderr = new StringBuilder();
            var stdout = new StringBuilder();
            try
            {
                string fnm = "";
                Args(ref fnm, ref cmd, output, dir);

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.FileName = fnm;
                startInfo.Arguments = cmd;
                startInfo.RedirectStandardInput = false;
                startInfo.RedirectStandardOutput = (output != Output.External);
                startInfo.RedirectStandardError = (output != Output.External);
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = (output != Output.External);
                if (!String.IsNullOrEmpty(dir) && output != Output.External){
                    startInfo.WorkingDirectory = dir;
                }

                using (Process process = Process.Start(startInfo))
                {
                    switch (output)
                    {
                        case Output.Internal:
                            _colorify.BlankLines();

                            while (!process.StandardOutput.EndOfStream) {
                                string line = process.StandardOutput.ReadLine();
                                stdout.AppendLine(line);
                                _colorify.Wrap($" {line}", txtPrimary);
                            }
                            
                            while (!process.StandardError.EndOfStream) {
                                string line = process.StandardError.ReadLine();
                                stderr.AppendLine(line);
                                _colorify.Wrap($" {line}", txtDanger);
                            }
                            break;
                        case Output.Hidden:
                            stdout.AppendLine(process.StandardOutput.ReadToEnd());
                            stderr.AppendLine(process.StandardError.ReadToEnd());
                            break;
                    }
                    
                    process.WaitForExit();
                    result.stdout = stdout.ToString();
                    result.stderr = stderr.ToString();
                    result.code = process.ExitCode;
                }
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex.Message);
            }
            return result;
        }

        public static void Browse(this string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception Ex)
            {
                switch (OS.GetCurrent())
                {
                    case "win":
                        url = url.Replace("&", "^&");
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                        break;
                    case "mac":
                    case "gnu":
                        Process.Start("open", url);
                        break;
                    default:
                        Message.Error(
                            msg: $" {Ex.Message}"
                        );
                        break;
                }
            }
        }

        public static void Result(string response){
            response = response
                .Replace("\r","")
                .Replace("\n","");
            if (!String.IsNullOrEmpty(response)){
                _colorify.WriteLine(response);
            } else {
                _colorify.WriteLine("is not installed", txtWarning);
            }
        }

        public static string GetWord(string request, int wordPosition){
            string response = "";
            try
            {
                string[] stringSeparators = new string[] { " " };
                string[] words = request.Split(stringSeparators, StringSplitOptions.None);
                if (!String.IsNullOrEmpty(request)){
                    response = words[wordPosition];
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }

        public static string[] SplitLines(string request) {
            string[] response = new string[] {};
            try
            {
                string[] stringSeparators = new string[] { Environment.NewLine, "\n" };
                string[] lines = request.Split(stringSeparators, StringSplitOptions.None);
                response = lines;
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }

        public static string ExtractLine(string request, string search, params string[] remove) {
            string response = "";
            try
            {
                string[] stringSeparators = new string[] { Environment.NewLine, "\n" };
                string[] lines = request.Split(stringSeparators, StringSplitOptions.None);
                foreach (string l in lines)
                {
                    if (l.Contains(search))
                    {
                        response = Strings.RemoveWords(l, remove);
                        break;
                    }
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return response;
        }
    }
}