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
        public static string Prefix { get; set; }

        public static int ScreenHeight { get; set;  }
        public static int ScreenWidth { get; set;  }

        // Variables for local server
        public static HestiaServerInteractor LocalServerinteractor { get; set; }
        public static String ServerName { get; set; }
        public static int Port { get; set; }
        public static String IP { get; set; }

        // Variables for global server
        public static HestiaServerInteractor ServerToAddDeviceTo { get; set; }
        public static List<HestiaServer> Auth0Servers { get; set; }

        public static List<HestiaServerInteractor> GetSelectedServers()
        {
            List<HestiaServerInteractor> serverInteractors = new List<HestiaServerInteractor>();
            foreach(HestiaServer auth0server in Auth0Servers)
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
                foreach (HestiaServerInteractor server in GetSelectedServers())
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
                temp_networkhandler = new NetworkHandler("https://" + IP, Port);
            }
            else
            {
                temp_networkhandler = new NetworkHandler(Auth0Servers[0].Interactor.NetworkHandler.Ip, Auth0Servers[0].Interactor.NetworkHandler.Port);
            }
            return temp_networkhandler;
        }

		public static HestiaServerInteractor GetTemporaryServerInteractor()
		{
			HestiaServerInteractor temp_serverinteractor;
            if (LocalLogin)
            {
                temp_serverinteractor = new HestiaServerInteractor(new NetworkHandler("https://" + IP, Port));
            }
            else
            {
				temp_serverinteractor = Auth0Servers[0].Interactor;
            }
			return temp_serverinteractor;
		}

        public static void ResetAllUserDefaults()
        {
            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;
            userDefaults.RemoveObject(strings.defaultsServerNameHestia);
            userDefaults.RemoveObject(strings.defaultsIpHestia);
            userDefaults.RemoveObject(strings.defaultsPortHestia);
            userDefaults.RemoveObject(strings.defaultsLocalHestia);
            userDefaults.RemoveObject(strings.defaultsAccessTokenHestia);
            userDefaults.RemoveObject(strings.defaultsIdentityTokenHestia);
        }

       // public static Reset
    }
}
