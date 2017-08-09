using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat {

    class Color {
        public static void Sample() {
            Colorify.Default();
            Console.Clear();
            
            var cp =  Program.config.personal;

            $"HARD HAT # 1.0.0 txtDefault {cp.ipl, 30} : {cp.hst, -20}".txtDefault(ct.WriteLine);
            $"HARD HAT # 1.0.0 txtMuted   {cp.ipl, 30} : {cp.hst, -20}".txtMuted(ct.WriteLine);
            $"HARD HAT # 1.0.0 txtPrimary {cp.ipl, 30} : {cp.hst}".txtPrimary(ct.WriteLine);
            $"HARD HAT # 1.0.0 txtSuccess {cp.ipl, 30} : {cp.hst}".txtSuccess(ct.WriteLine);
            $"HARD HAT # 1.0.0 txtInfo    {cp.ipl, 30} : {cp.hst}".txtInfo(ct.WriteLine);
            $"HARD HAT # 1.0.0 txtWarning {cp.ipl, 30} : {cp.hst}".txtWarning(ct.WriteLine);
            $"HARD HAT # 1.0.0 txtDanger  {cp.ipl, 30} : {cp.hst}".txtDanger(ct.WriteLine);
            $"HARD HAT # 1.0.0 bgDefault  {cp.ipl, 30} : {cp.hst}".bgDefault(ct.WriteLine);
            $"HARD HAT # 1.0.0 bgMuted    {cp.ipl, 30} : {cp.hst}".bgMuted(ct.WriteLine);
            $"HARD HAT # 1.0.0 bgPrimary  {cp.ipl, 30} : {cp.hst}".bgPrimary(ct.WriteLine);
            $"HARD HAT # 1.0.0 bgSuccess  {cp.ipl, 30} : {cp.hst}".bgSuccess(ct.WriteLine);
            $"HARD HAT # 1.0.0 bgInfo     {cp.ipl, 30} : {cp.hst}".bgInfo(ct.WriteLine);
            $"HARD HAT # 1.0.0 bgWarning  {cp.ipl, 30} : {cp.hst}".bgWarning(ct.PadLeft);
            $"HARD HAT # 1.0.0 bgDanger   {cp.ipl, 30} : {cp.hst}".bgDanger(ct.PadRight);

            string opt = Console.ReadLine();
            Menu.Start();
        }
    }
}