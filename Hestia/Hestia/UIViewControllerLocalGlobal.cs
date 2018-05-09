using Foundation;
using System;
using UIKit;
using Hestia.backend.utils;
using Hestia.backend;
using Hestia.Resources;
using Hestia.DevicesScreen.resources;

namespace Hestia
{   // This view controller belongs to the first window that can be shown on loading the app
    // if no user defaults are present. You can then choose local/global
    public partial class UIViewControllerLocalGlobal : UIViewController
    {
        string defaultServerName;
        string defaultIP;
        string defaultPort;
        NSUserDefaults userDefaults;
        public UIViewControllerLocalGlobal (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            userDefaults = NSUserDefaults.StandardUserDefaults;
            //userDefaults.RemoveObject(strings.defaultsServerNameHestia);
            //userDefaults.RemoveObject(strings.defaultsIpHestia);
            //userDefaults.RemoveObject(strings.defaultsPortHestia);
        }

		public override void ViewDidAppear(bool animated)
		{
            base.ViewDidAppear(animated);
            bool validIp = false;

            defaultServerName = userDefaults.StringForKey(strings.defaultsServerNameHestia);
            defaultIP = userDefaults.StringForKey(strings.defaultsIpHestia);
            defaultPort = userDefaults.StringForKey(strings.defaultsPortHestia);
            string defaultLocal = userDefaults.StringForKey(strings.defaultsLocalHestia);

            // Already anticipate local login
            // Check if local serverinformation is present and correct
            if (defaultServerName != null && defaultIP != null && defaultPort != null)
            {
                validIp = PingServer.Check(defaultIP, int.Parse(defaultPort));
            }

            // Check if a last startup option is unknown
            if (defaultLocal == null)
            {
                ToLocalButton.TouchUpInside += delegate (object sender, EventArgs e)
                {
                    userDefaults.SetString(bool.TrueString, strings.defaultsLocalHestia);

                    if (validIp)
                    {
                        SetValuesAndSegueToDevicesMain();
                    }
                    else
                    {
                        Console.WriteLine("To serverconnect");
                        PerformSegue(strings.mainToServerConnect, this);
                    }
                };

                ToGlobalButton.TouchUpInside += delegate (object sender, EventArgs e)
                {
                    Globals.LocalLogin = false;
                    userDefaults.SetString(bool.FalseString, strings.defaultsLocalHestia);

                    Console.WriteLine("To auth0login");
                    //PerformSegue(strings.mainToServerConnect, this);
                };
            }
            // Last login was local
            else if (bool.Parse(defaultLocal))
            {
                if (validIp)
                {
                    SetValuesAndSegueToDevicesMain();
                }
                else
                {
                    Console.WriteLine("To serverconnect");
                    PerformSegue(strings.mainToServerConnect, this);
                }
            }
            else // Previous time global login
            {
                // Should ultimately go direct to multiple server screen
                Console.WriteLine("To auth0login");
                PerformSegue(strings.mainToAuth0, this);
            }
        
		}

        void SetValuesAndSegueToDevicesMain()
        {
            Globals.LocalLogin = true;
            Globals.ServerName = defaultServerName;
            Globals.IP = defaultIP;
            Globals.Port = int.Parse(defaultPort);
            Globals.LocalServerinteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
            Console.WriteLine("To devices main");
            PerformSegue(strings.mainToDevicesMain, this);
        }
	}
}
