using System;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Linq;
using ToolBox.Transform;

namespace dein.tools
{
    public static class Network
    {
        public static string GetLocalIPAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            return localIP;
        }

        public static string GetLocalIPBase(string ip)
        {
            ip = Strings.RemoveWords(ip, $".{ip.Split('.').Last()}");
            return $"{ip}.";
        }
    }
}