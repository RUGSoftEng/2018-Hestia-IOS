using Hestia.backend.exceptions;
using System;

namespace Hestia.backend.utils
{
    class PingServer
    {
        public static bool Check(string address, int port)
        {
            HestiaServerInteractor interactor = new HestiaServerInteractor(new NetworkHandler(address, port));
            try
            {
                interactor.GetDevices();
                return true;
            }
            catch (ServerInteractionException)
            {
                Console.Out.Write("No such server exists");
                return false;
            }
        }
    }
}