using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
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

        public static int ScreenHeight { get; set; }
        public static int ScreenWidth { get; set; }

        // Variables for local server
        public static HestiaServerInteractor LocalServerinteractor { get; set; }
        public static String ServerName { get; set; }
        public static String Address { get; set; }

        // Variables for global server
        public static HestiaServerInteractor ServerToAddDeviceTo { get; set; }
        public static List<HestiaServer> Auth0Servers { get; set; }
        public static HestiaWebServerInteractor HestiaWebServerInteractor { get; set; }
        public static List<HestiaServerInteractor> GetInteractorsOfSelectedServers()
        {
            List<HestiaServerInteractor> serverInteractors = new List<HestiaServerInteractor>();
            foreach (HestiaServer auth0server in Auth0Servers)
            {
                if (auth0server.Selected)
                {
                    serverInteractors.Add(auth0server.Interactor);
                }
            }
            return serverInteractors;
        }

        public static List<HestiaServer> GetSelectedServers()
        {
            List<HestiaServer> servers = new List<HestiaServer>();
            foreach (HestiaServer auth0server in Auth0Servers)
            {
                if (auth0server.Selected)
                {
                    servers.Add(auth0server);
                }
            }
            return servers;
        }

        public static nint GetNumberOfSelectedServers()
        {
            return GetInteractorsOfSelectedServers().Count;
        }

        public static void ResetAllUserDefaults()
        {
            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;
            userDefaults.RemoveObject(strings.defaultsServerNameHestia);
            userDefaults.RemoveObject(strings.defaultsIpHestia);
            userDefaults.RemoveObject(strings.defaultsPortHestia);
            userDefaults.RemoveObject(strings.defaultsLocalHestia);
            userDefaults.RemoveObject(strings.defaultsAccessTokenHestia);
        }
    }
}