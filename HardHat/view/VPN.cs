using System;
using System.Runtime.InteropServices;
using dein.tools;
using static HardHat.Program;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    public static partial class Vpn {


        public static void Verification() {
            try
            {
                if (
                    Variables.Valid("bv") &&
                    !Vpn.CmdStatus(_config.vpn.snm, Variables.Value("bv"))
                )
                {
                    Vpn.CmdDisconnect(Variables.Value("bv"));
                    Vpn.CmdConnect(_config.vpn.snm, Variables.Value("bv"));
                    Message.Alert(" Please connect your VPN and try again.");
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}