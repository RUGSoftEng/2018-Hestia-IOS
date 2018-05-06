using Hestia.backend.exceptions;
using System;

namespace Hestia.backend.utils
{
    class PingServer
    {
        public static bool Check(string address, int port)
        {
            ServerInteractor interactor = new ServerInteractor(new NetworkHandler(address, port));
            try
            {
                interactor.GetDevices();
                return true;
            }
            catch (ServerInteractionException ex)
            {
                Console.Out.Write("No such server exists");
                return false;
            }
        }
    }
}