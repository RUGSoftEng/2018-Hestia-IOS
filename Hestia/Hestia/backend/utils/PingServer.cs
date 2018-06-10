using Hestia.Backend.Exceptions;
using System;

namespace Hestia.Backend.Utils
{
    class PingServer
    {
        public static bool Check(string address)
        {
            HestiaServerInteractor interactor = new HestiaServerInteractor(new NetworkHandler(address));
            try
            {
                interactor.GetDevices();
                return true;
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("No such server exists");
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}