using Hestia.Backend.Exceptions;
using System;

namespace Hestia.Backend.Utils
{
    /// <summary>
    /// This class contains a method which verifies if a hestia server exists.
    /// </summary>
    class PingServer
    {
        /// <summary>
        /// Checks if a hestia server exists.
        /// </summary>
        /// <param name="address"></param>
        /// <returns>True or false</returns>
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
