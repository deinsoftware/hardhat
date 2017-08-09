using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static partial class VPN {
        public static bool CmdStatus(string sitename, string dir) {
            bool cnt = false;
            try
            {
                Response result = new Response();
                switch (OS.WhatIs())
                {
                    case "win":
                        result = $"trac info -s {sitename} -tr true | findstr status".Term(Output.Hidden, dir);
                        break;
                    case "mac":
                        result = $"trac info -s {sitename} -tr true | egrep -i 'status:'".Term(Output.Hidden, dir);
                        break;
                }
                result.stdout = result.stdout
                    .Replace("\r","")
                    .Replace("\n","");
                cnt = (result.code == 0) && (!String.IsNullOrEmpty(result.stdout) && result.stdout.Contains("Connected"));
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return cnt;
        }

        public static void CmdDisconnect(string dir) {
            try
            {
                $"trac disconnect".Term(Output.Hidden, dir);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdConnect(string sitename, string dir) {
            try
            {
                $"trac connectgui -s {sitename}".Term(Output.Hidden, dir);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}