using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static HardHat.Program;

namespace dein.tools
{
    public static class Paths
    {
        public static void Exists(this string path, string message = null)
        {
            try
            {
                if (!_fileSystem.DirectoryExists(path))
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" Path not found:{Environment.NewLine}");
                    msg.Append($" '{path}'{Environment.NewLine}");
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
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
        }

        public static List<string> Directories(this string path, string filter, string type)
        {
            List<string> dirs = new List<string>();
            try
            {
                dirs = _path.GetDirectories(path, filter);
                if (dirs.Count < 1)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" There is no {type} in current location:{Environment.NewLine}");
                    msg.Append($" '{path}'");

                    Message.Alert(msg.ToString());
                }
            }
            catch (UnauthorizedAccessException UAEx)
            {
                Exceptions.General(UAEx);
            }
            catch (PathTooLongException PathEx)
            {
                Exceptions.General(PathEx);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return dirs;
        }

        public static List<string> Files(this string path, string filter, string message = null)
        {
            List<string> files = new List<string>();
            try
            {
                files = _path.GetFiles(path, filter);
                if (files.Count < 1)
                {
                    StringBuilder msg = new StringBuilder();
                    msg.Append($" There is no files in current location.{Environment.NewLine}");
                    msg.Append($" '{path}'{Environment.NewLine}");
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
                Exceptions.General(UAEx);
            }
            catch (PathTooLongException PathEx)
            {
                Exceptions.General(PathEx);
            }
            catch (Exception Ex)
            {
                Exceptions.General(Ex);
            }
            return files;
        }
    }
}