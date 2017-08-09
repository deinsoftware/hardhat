using System;
using System.Runtime.InteropServices;
using dein.tools;

using ct = dein.tools.Colorify.Type;

namespace HardHat 
{
    static partial class VPN {
        public static void Verification() {
            var c = Program.config;
            var cp = Program.config.personal;
            try
            {
                if (cp.mnu.v_env)
                {
                    if (!VPN.CmdStatus(c.vpn.snm, Env.Get("VPN_HOME")))
                    {
                        VPN.CmdDisconnect(Env.Get("VPN_HOME"));
                        VPN.CmdConnect(c.vpn.snm, Env.Get("VPN_HOME"));
                        Message.Alert(" Please connect your VPN and try again.");
                    }
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