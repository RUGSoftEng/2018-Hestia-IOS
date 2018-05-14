using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using System.Drawing;

using System.Collections;
using CoreGraphics;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.Resources;

namespace Hestia.DevicesScreen.resources
{
    public static class Globals
    {
        public static bool LocalLogin { get; set; }
        public static UIColor DefaultLightGray { get; set; }
        public static String UserName { get; set; }

        public static int ScreenHeight { get; set;  }
        public static int ScreenWidth { get; set;  }

        // Variables for local server
        public static ServerInteractor LocalServerinteractor { get; set; }
        public static String ServerName { get; set; }
        public static int Port { get; set; }
        public static String IP { get; set; }

        // Variables for global server
        public static ServerInteractor ServerToAddDeviceTo { get; set; }
        public static List<WebServer> Auth0Servers { get; set; }

        public static List<ServerInteractor> GetSelectedServers()
        {
            List<ServerInteractor> serverInteractors = new List<ServerInteractor>();
            foreach(WebServer auth0server in Auth0Servers)
            {
                if(auth0server.Selected)
                {
                    serverInteractors.Add(auth0server.Interactor);
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
                temp_networkhandler = new NetworkHandler(Auth0Servers[0].Interactor.NetworkHandler.Ip, Auth0Servers[0].Interactor.NetworkHandler.Port);
            }
            return temp_networkhandler;
        }

        public static void ResetUserDefaults()
        {
            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;
            userDefaults.RemoveObject(strings.defaultsServerNameHestia);
            userDefaults.RemoveObject(strings.defaultsIpHestia);
            userDefaults.RemoveObject(strings.defaultsPortHestia);
            userDefaults.RemoveObject(strings.defaultsLocalHestia);
        }
    }
}
