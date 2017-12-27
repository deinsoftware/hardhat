using System.Runtime.InteropServices;
using System;
using System.Diagnostics;

namespace dein.tools
{
    public static class Os
    {
        public static bool IsWindows() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsMacOS() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static bool IsLinux() =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static string Platform() {
            var os = (Os.IsWindows() ? "win" : null) ??
                    (Os.IsMacOS()    ? "mac" : null) ??
                    (Os.IsLinux()    ? "gnu" : null) ;
            return os;
        }
    }
}