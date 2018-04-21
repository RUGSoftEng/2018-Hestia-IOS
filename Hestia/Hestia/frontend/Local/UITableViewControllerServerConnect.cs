using Foundation;
using ObjCRuntime;
using System;
using UIKit;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.DevicesScreen
{


    public partial class UITableViewControllerServerConnect : UITableViewController
    {

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
            
        }

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            newServerName.Text = "Hestia_Server2018";
            newIP.Text = "94.212.164.28";
            newPort.Text = "8000";

        }

		public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
		{
            if (segueIdentifier == "ServerToDevices")
            {
                if (newIP.Text == "94.212.164.28" && newPort.Text == "8000")
                {
                    Globals.ServerName = newServerName.Text;
                    Globals.IP = newIP.Text;
                    Globals.Port = int.Parse(newPort.Text);
                    ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                    Globals.LocalServerInteractor = serverInteractor;
                    return true;
                }
                else
                {
                    UIAlertView alert = new UIAlertView()
                    {
                        Title = "Could not connect to server",
                        Message = "Enter correct IP and Port"
                    };
                    alert.AddButton("OK");
                    alert.Show();
                    return false;
                }
            }
            else
            {
                return true;
            }
		}
	}
}