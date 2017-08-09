using System.Runtime.InteropServices;
using System;
using System.Diagnostics;
using System.Text;

using ct = dein.tools.Colorify.Type;
using System.Reflection;
using System.IO;

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
                switch (OS.WhatIs())
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
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
                startInfo.RedirectStandardOutput = !(output == Output.External);
                startInfo.RedirectStandardError = !(output == Output.External);
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = !(output == Output.External);
                if (!String.IsNullOrEmpty(dir) && output != Output.External){
                    startInfo.WorkingDirectory = dir;
                }
                //TODO: Add Standar Input to Cancel process

                using (Process process = Process.Start(startInfo))
                {
                    switch (output)
                    {
                        case Output.Internal:
                            $"".fmNewLine();

                            while (!process.StandardOutput.EndOfStream) {
                                string line = process.StandardOutput.ReadLine();
                                stdout.AppendLine(line);
                                $"{line}".txtPrimary(ct.Shell);
                            }
                            
                            while (!process.StandardError.EndOfStream) {
                                string line = process.StandardError.ReadLine();
                                stderr.AppendLine(line);
                                $"{line}".txtDanger(ct.Shell);
                            }
                            break;
                        case Output.Hidden:
                            stdout.AppendLine(process.StandardOutput.ReadToEnd());
                            stderr.AppendLine(process.StandardError.ReadToEnd());
                            break;
                    }
                    if (output == Output.External){
                        //process.Kill();
                    }
                    process.WaitForExit();
                    result.stdout = stdout.ToString();
                    result.stderr = stderr.ToString();
                    result.code = process.ExitCode;
                }
            }
            catch (Exception Ex)
            {
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return result;
        }

        public static void Result(string response){
            response = response
                .Replace("\r","")
                .Replace("\n","");
            if (!String.IsNullOrEmpty(response)){
                $"{response}".txtDefault(ct.WriteLine);
            } else {
                $"is not installed".txtWarning(ct.WriteLine);
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
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
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }

        public static string RemoveLine(string request, string search, params string[] remove) {
            string response = "";
            try
            {
                string[] stringSeparators = new string[] { Environment.NewLine, "\n" };
                string[] lines = request.Split(stringSeparators, StringSplitOptions.None);
                foreach (string l in lines)
                {
                    if (l.Contains(search))
                    {
                        response = Strings.Remove(l, remove);
                        break;
                    }
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }
    }
}