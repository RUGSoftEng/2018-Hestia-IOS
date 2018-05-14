using Foundation;
using System;
using System.Collections.Generic;
using UIKit;
using Hestia.backend;
using Hestia.DevicesScreen.resources;
using Hestia.backend.models;

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
            Globals.FirebaseServers = new List<HestiaServer>();
            // suppose list of server is retrieved from server
            // a list with ip addresses and ports
            // This is a temporary solution to show the functionality
            // Should be replaced with list from webserver
            List<string> ipadresses = new List<string>
            {   Resources.strings.ipDaan, 
                Resources.strings.ipDaan,
                Resources.strings.ipDaan };

            foreach(string ip in ipadresses)
            {
                var networkhandler = new NetworkHandler(ip, int.Parse(Resources.strings.defaultPort));
                var serverinteractor = new HestiaServerInteractor(networkhandler);
                var firebaseserver = new HestiaServer(true, serverinteractor);
                firebaseserver.Selected = false;
                Globals.FirebaseServers.Add(firebaseserver);
            }
        }
    }
}
