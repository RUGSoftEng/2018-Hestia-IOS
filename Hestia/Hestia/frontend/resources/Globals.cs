﻿using System;
using System.Collections.Generic;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.DevicesScreen.resources
{
    public static class Globals
    {
        public static bool LocalLogin { get; set; }

        public static ServerInteractor LocalServerInteractor { get; set; }
        public static String ServerName { get; set; }
        public static int Port { get; set; }
        public static String IP { get; set; }

        // In case of Firebase login
        public static List<NetworkHandler> FirebaseNetworkHandlers { get; set; }
        public static List<ServerInteractor> FirebaseServerInteractors { get; set; }

        // Get devices for only local servers or all Firebase Servers
        public static List<Device> getDevices()
        {
            List<Device> devices = new List<Device>();

            if (LocalLogin)
            {
                devices = LocalServerInteractor.GetDevices();
            }
            else
            {
                foreach (ServerInteractor interactor in FirebaseServerInteractors)
                {
                    devices.AddRange(interactor.GetDevices());
                }
            }

            return devices;
        }
    
    }
}
