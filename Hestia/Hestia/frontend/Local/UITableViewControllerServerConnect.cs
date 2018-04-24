using Foundation;
using ObjCRuntime;
using System;
using UIKit;

using Hestia.DevicesScreen.resources;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.backend.utils;

namespace Hestia.DevicesScreen
{


    public partial class UITableViewControllerServerConnect : UITableViewController
    {

        private bool debug = false;

        public UITableViewControllerServerConnect(IntPtr handle) : base(handle)
        {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override bool ShouldPerformSegue(string segueIdentifier, NSObject sender)
        {

           bool validIp = false;
           if (debug)
                {
                    newServerName.Text = "Hestia_Server2018";
                    newIP.Text = "94.212.164.28";
                    newPort.Text = "8000";
                    Globals.ServerName = newServerName.Text;
                    Globals.IP = newIP.Text;
                    Globals.Port = int.Parse(newPort.Text);
                    ServerInteractor serverInteractor = new ServerInteractor(new NetworkHandler(Globals.IP, Globals.Port));
                    Globals.LocalServerInteractor = serverInteractor;
                    return true;
                }
            else
            {
                try
                {
                    validIp = PingServer.Check(newIP.Text, int.Parse(newPort.Text));
                }
                catch (Exception exception)
                {
                    Console.Write(exception.StackTrace);
                    displayWarningMessage();
                    return false;
                }
                if (validIp)
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
                    displayWarningMessage();
                    return false;
                }
            }
        }

        public void displayWarningMessage()
        {
            UIAlertView alert = new UIAlertView()
            {
                Title = "Could not connect to server",
                Message = "Invalid server information"
            };
            alert.AddButton("OK");
            alert.Show();
            connectButton.Selected = false;
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
        }
    }
}