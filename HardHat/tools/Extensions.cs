using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace dein.tools
{
    public static class Paths
    {
        public static void Exists(this string dir, string message = null)
        {
            try
            {
                if (!Directory.Exists(dir)){
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" Path not found:{Environment.NewLine}");
                    msg.Append($" '{dir}'{Environment.NewLine}");
                    if (!String.IsNullOrEmpty(message))
                    {
                        msg.Append(Environment.NewLine);
                        msg.Append($" {message}");
                    }

                    Message.Error(
                        msg: msg.ToString()
                    );
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }

        public static List<string> Directories(this string dir, string flt, string type){
            List<string> dirs = new List<string>();
            try
            {
                dirs = new List<string>(Directory.EnumerateDirectories(dir, flt).OrderBy(name => name));
                if (dirs.Count < 1)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" There is no {type} in current location:{Environment.NewLine}");
                    msg.Append($" '{dir}'");
                    
                    Message.Alert(msg.ToString());
                }
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Exceptions.General(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Exceptions.General(PathEx.Message);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return dirs;
        }
    
        public static List<string> Files(this string dir, string flt, string message = null){
            List<string> files = new List<string>();
            try
            {
                files = new List<string>(Directory.EnumerateFiles(dir, flt).OrderBy(name => name));
                if (files.Count < 1)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" There is no files in current location.{Environment.NewLine}");
                    msg.Append($" '{dir}'{Environment.NewLine}");
                    if (!String.IsNullOrEmpty(message))
                    {
                        msg.Append(Environment.NewLine);
                        msg.Append($" {message}");
                    }

                    Message.Alert(msg.ToString());
                }
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Exceptions.General(UAEx.Message);
            }
            catch (PathTooLongException PathEx)
            {
                Exceptions.General(PathEx.Message);
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
            return files;
        }
    }
}