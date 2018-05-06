using Foundation;
using System;
using UIKit;
using Hestia.backend.utils;
using Hestia.backend.models;
using Hestia.backend;
using Hestia.Resources;
using Hestia.DevicesScreen.resources;

namespace Hestia
{
    public partial class UIViewControllerMain : UIViewController
    {
        partial void ButtonPressed(UIButton sender)
        {
            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;
            string defaultServerName = userDefaults.StringForKey(Resources.strings.defaultsServerNameHestia);
            string defaultIP = userDefaults.StringForKey(Resources.strings.defaultsIpHestia);
            string defaultPort = userDefaults.StringForKey(Resources.strings.defaultsPortHestia);

            if (defaultServerName != null && defaultIP != null && defaultPort != null)
            {
                bool validIp = false;
                validIp = PingServer.Check(defaultIP, int.Parse(defaultPort));

                if (validIp)
                {
                    Globals.ServerName = defaultServerName;
                    Globals.IP = defaultIP;
                    Globals.Port = int.Parse(defaultPort);
                    ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                    Globals.LocalServerinteractor = serverInteractor;
                    PerformSegue("mainToDevicesMain", this);

                }
                else
                {
                    PerformSegue("mainToServerConnect", this);
                }
            }
        }

        public UIViewControllerMain (IntPtr handle) : base (handle)
        {
        }
    }
}