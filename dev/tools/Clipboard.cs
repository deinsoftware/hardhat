using System.Runtime.InteropServices;
using System;
using System.Diagnostics;

namespace dein.tools
{
    public static class Clipboard
    {
        public static void Copy(string val)
        {
            switch (OS.WhatIs())
            {
                case "win":
                    $"echo {val} | clip".Term();
                    break;
                case "mac":
                    $"echo \"{val}\" | pbcopy".Term();
                    break;
            }
        }
    }
}