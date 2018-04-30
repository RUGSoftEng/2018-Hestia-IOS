using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using Hestia.backend;
using Hestia.DevicesScreen.resources;

namespace Hestia
{
    public partial class UIViewControllerGlobalLogin : UIViewController
    {
        
        public UIViewControllerGlobalLogin (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Globals.FirebaseServers = new List<FireBaseServer>();
            // suppose list of server is retrieved from server
            // a list with ip addresses and ports
            List<string> ipadresses = new List<string>
            {"94.212.164.28", "94.212.164.28","94.212.164.28" };
            List<int> ports = new List<int> { 8000, 8000, 8000 };

            foreach(string ip in ipadresses)
            {
                var networkhandler = new NetworkHandler(ip, 8000);
                var serverinteractor = new ServerInteractor(networkhandler);
                var fbserver = new FireBaseServer(true, serverinteractor);
                Globals.FirebaseServers.Add(fbserver);
            }
        }
    }
}