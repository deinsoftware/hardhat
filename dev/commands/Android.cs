using System;
using System.Runtime.InteropServices;
using System.Text;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static partial class ADB{
        public static bool CmdDevices() {
            bool dev = false;
            try
            {
                Response result = new Response();
                switch (OS.WhatIs())
                {
                    case "win":
                        result = $"adb devices | findstr \"\\<device\\>\"".Term();
                        break;
                    case "mac":
                        result = $"adb devices -l | egrep -i 'device usb:|device product:'".Term();
                        break;
                }
                result.stdout = result.stdout
                    .Replace("\r","")
                    .Replace("\n","");
                dev = (result.code == 0) && (!String.IsNullOrEmpty(result.stdout));
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return dev;
        }

        public static void CmdInstall(string path, string device){
            try
            {
                StringBuilder cmd = new StringBuilder();
                cmd.Append("adb");
                if (!String.IsNullOrEmpty(device))
                {
                    Response result = new Response();
                    result = $"adb -s {device} get-state".Term();
                    if (result.stdout.Contains("device")){
                        cmd.Append($" -s {device}");
                    } else {
                        Message.Critical(
                            msg: $" Device '{device}' not found."
                        );
                    }
                }
                cmd.Append($" -r {path} 2>&1");
                cmd.ToString().Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static bool CmdConnect(string ip, string port){
            bool connected = false;
            try
            {
                Response result = new Response();
                result = $"adb connect {ip}:{port}".Term(Output.Internal);
                if (result.stdout.StartsWith($"connected to {ip}:{port}")){
                    connected = true;
                } else {
                    connected = false;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return connected;
        }

        public static bool CmdDisconnect(string ip, string port){
            bool connected = true;
            try
            {
                
                Response result = new Response();
                result = $"adb disconnect {ip}:{port}".Term(Output.Internal);;
                if (result.stdout.StartsWith($"disconnected {ip}:{port}")){
                    connected = false;
                } else {
                    connected = true;
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return connected;
        }

        public static void CmdKillServer(){
            try
            {
                $"adb kill-server".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdStartServer(){
            try
            {
                $"adb start-server".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static string CmdList() {
            string response = "";
            try
            {
                Response result = new Response();
                result = $"adb devices -l".Term();
                response = Strings.Remove(result.stdout, $"List of devices attached{Environment.NewLine}");
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
            return response;
        }
    }

    static partial class BuildTools {
        public static void CmdSignerVerify(string path){
            try
            {
                $"apksigner verify --print-certs {path}".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }

        public static void CmdInformation(string path){
            try
            {
                $"aapt dump badging {path}".Term(Output.Internal);
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}