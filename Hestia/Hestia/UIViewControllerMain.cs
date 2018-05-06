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
        public UIViewControllerMain(IntPtr handle) : base(handle)
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            bool validIp = false;

            // Already anticipate local login
            NSUserDefaults userDefaults = NSUserDefaults.StandardUserDefaults;
            string defaultServerName = userDefaults.StringForKey(Resources.strings.defaultsServerNameHestia);
            string defaultIP = userDefaults.StringForKey(Resources.strings.defaultsIpHestia);
            string defaultPort = userDefaults.StringForKey(Resources.strings.defaultsPortHestia);

            if (defaultServerName != null && defaultIP != null && defaultPort != null)
            {
                validIp = PingServer.Check(defaultIP, int.Parse(defaultPort));
            }

            LocalSignInButton.TouchUpInside += delegate (object sender, EventArgs e)
            {
                Globals.LocalLogin = true;
                    if (validIp)
                    {
                        Globals.ServerName = defaultServerName;
                        Globals.IP = defaultIP;
                        Globals.Port = int.Parse(defaultPort);
                        ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                        Globals.LocalServerinteractor = serverInteractor;
                        Console.WriteLine("To devices main");
                        PerformSegue("mainToDevicesMain", this);
                    }
                    else
                    {
                          Console.WriteLine("To serverconnect");
                        PerformSegue("mainToServerConnect", this);
                        
                    }
            };
        }
   
    }
}