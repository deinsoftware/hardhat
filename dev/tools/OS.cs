using System.Runtime.InteropServices;
using System;
using System.Diagnostics;

namespace dein.tools
{
    public static class OS
    {
        public static bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static string WhatIs() {
            var os = (OS.IsWindows() ? "win" : null) ??
                    (OS.IsMacOS()    ? "mac" : null) ??
                    (OS.IsLinux()    ? "gnu" : null) ;
            return os;
        }
    }
}