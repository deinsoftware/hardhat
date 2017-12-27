using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    public static partial class Vpn {
        
        private static Config _c { get; set; }
        private static PersonalConfiguration _cp { get; set; }

        static Vpn()
        {
            _c = Program._config;
            _cp = Program._config.personal;
        }

        public static void Verification() {
            try
            {
                if (
                    Variables.Valid("bv") &&
                    !Vpn.CmdStatus(_c.vpn.snm, Variables.Value("bv"))
                )
                {
                    Vpn.CmdDisconnect(Variables.Value("bv"));
                    Vpn.CmdConnect(_c.vpn.snm, Variables.Value("bv"));
                    Message.Alert(" Please connect your VPN and try again.");
                }
            }
            catch (Exception Ex){
                Exceptions.General(Ex.Message);
            }
        }
    }
}