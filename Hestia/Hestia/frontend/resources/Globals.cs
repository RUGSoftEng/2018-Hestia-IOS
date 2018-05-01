using System;
using System.Collections.Generic;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.DevicesScreen.resources
{
    public static class Globals
    {
        public static bool LocalLogin { get; set; }
        public static ServerInteractor LocalServerinteractor { get; set; }
        public static String ServerName { get; set; }
        public static int Port { get; set; }
        public static String IP { get; set; }
        public static String userName { get; set; }

        public static ServerInteractor serverToAddDeviceTo { get; set; }

        // In case of Firebase login
        public static List<FireBaseServer> FirebaseServers { get; set; }

        public static List<ServerInteractor> GetSelectedServers()
        {
            List<ServerInteractor> serverInteractors = new List<ServerInteractor>();
            foreach(FireBaseServer firebaseserver in FirebaseServers)
            {
                if(firebaseserver.Selected)
                {
                    serverInteractors.Add(firebaseserver.Interactor);
                }
            }
            return serverInteractors;
        }

        public static nint GetNumberOfSelectedServers()
        {
            return GetSelectedServers().Count;
        }

        // Get devices for only local servers or selected Firebase Servers
        public static List<Device> GetDevices()
        {
            List<Device> devices = new List<Device>();
            if (LocalLogin)
            {
                devices = LocalServerinteractor.GetDevices();
            }
            else
            {
                foreach (ServerInteractor server in GetSelectedServers())
                {
                    devices.AddRange(server.GetDevices());
                }
            }
            return devices;
        }

        public static NetworkHandler GetTemporyNetworkHandler()
        {
            NetworkHandler temp_networkhandler;
            if(LocalLogin)
            {
                temp_networkhandler = new NetworkHandler(IP, Port);
            }
            else
            {
                temp_networkhandler = new NetworkHandler(FirebaseServers[0].Interactor.NetworkHandler.Ip, FirebaseServers[0].Interactor.NetworkHandler.Port);
            }
            return temp_networkhandler;
        }
    }
}
