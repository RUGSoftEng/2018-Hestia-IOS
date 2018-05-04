using System;
using System.Net.Sockets;

namespace Hestia.backend.utils
{
    class PingServer
    {
        public static bool Check(string address, int port)
        {
            using (TcpClient client = new TcpClient())
            {
                try
                {
                    client.Connect(address, port);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}