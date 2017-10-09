using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static partial class Vpn {
        
        public static void Verification() {
            var c = Program.config;
            var cp = Program.config.personal;
            try
            {
                if (
                    cp.mnu.v_env &&
                    !Vpn.CmdStatus(c.vpn.snm, dein.tools.Env.Get("VPN_HOME"))
                )
                {
                    Vpn.CmdDisconnect(dein.tools.Env.Get("VPN_HOME"));
                    Vpn.CmdConnect(c.vpn.snm, dein.tools.Env.Get("VPN_HOME"));
                    Message.Alert(" Please connect your VPN and try again.");
                }
            }
            catch (Exception Ex){
                Message.Critical(
                    msg: $" {Ex.Message}"
                );
            }
        }
    }
}