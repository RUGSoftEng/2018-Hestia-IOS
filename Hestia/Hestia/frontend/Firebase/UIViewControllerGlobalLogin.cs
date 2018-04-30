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

        // This should be moved to the serverList class later
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Globals.LocalLogin = false;
            Globals.FirebaseServers = new List<FireBaseServer>();
            // suppose list of server is retrieved from server
            // a list with ip addresses and ports
            List<string> ipadresses = new List<string>
            {"94.212.164.28", "94.212.164.28","94.212.164.28" };


            foreach(string ip in ipadresses)
            {
                var networkhandler = new NetworkHandler(ip, 8000);
                var serverinteractor = new ServerInteractor(networkhandler);
                var fbserver = new FireBaseServer(true, serverinteractor);
                fbserver.Selected = false;
                Globals.FirebaseServers.Add(fbserver);
            }
        }
    }
}