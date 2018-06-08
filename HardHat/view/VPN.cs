using System;
using System.Runtime.InteropServices;
using dein.tools;
using static HardHat.Program;

namespace HardHat 
{
    public static partial class Vpn {


        public static void Verification() {
            try
            {
                if (
                    Variables.Valid("bv") &&
                    !Vpn.CmdStatus(_config.vpn.siteName, Variables.Value("bv"))
                )
                {
                    Vpn.CmdDisconnect(Variables.Value("bv"));
                    Vpn.CmdConnect(_config.vpn.siteName, Variables.Value("bv"));
                    Message.Alert(" Please connect your VPN and try again.");
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex);
            }
        }
    }
}