using System;
using UIKit;
using CoreGraphics;
using System.Collections;
using Hestia.DevicesScreen.resources;
using Hestia.backend.models;
using Hestia.backend.exceptions;
using Hestia.frontend;
using System.Text.RegularExpressions;
using Hestia.backend;
using Hestia.Resources;

namespace Hestia
{
    public partial class AddServerViewController : UIViewController
    {
        UIBarButtonItem done;
      
        public AddServerViewController (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();

            NetworkHandler network = new NetworkHandler(strings.webserverIP, strings.defaultsAccessTokenHestia);
            HestiaWebServerInteractor serverToAdd = new HestiaWebServerInteractor(network);
            Regex rxIP = new Regex(@"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}");
            Regex rxName = new Regex(@"^(.)+$");
            MatchCollection matchesName, matchesIP;


            // Rectangular cell displaying the label and inputfield
            UIView rectangle = new UIView(new CGRect(0, 90, View.Bounds.Width, 50));
            rectangle.BackgroundColor = UIColor.White;
            View.AddSubview(rectangle);

            // Label
            UILabel newName = new UILabel();
            newName.Text = "New name:";
            newName.Frame = new CGRect(15, 10, 100, 31);
            rectangle.AddSubview(newName);

            // Inputfield
            UITextField changeNameField = new UITextField();
            changeNameField.Frame = new CGRect(110, 10, View.Bounds.Width - 125, 31);
            changeNameField.Placeholder = "Introduce a name";
            rectangle.AddSubview(changeNameField);

            // Rectangular cell displaying the label and inputfield
            UIView rectangle2 = new UIView(new CGRect(0, 140, View.Bounds.Width, 50));
            rectangle2.BackgroundColor = UIColor.White;
            View.AddSubview(rectangle2);
                
            // Label
            UILabel newIP = new UILabel();
            newIP.Text = "New IP:";
            newIP.Frame = new CGRect(15, 10, 100, 31);
            rectangle2.AddSubview(newIP);

            // Inputfield
            UITextField changeIPField = new UITextField();
            changeIPField.Frame = new CGRect(110, 10, View.Bounds.Width - 125, 31);
            changeIPField.Placeholder = "Introduce an IP";
            rectangle2.AddSubview(changeIPField);

            // Rectangular cell displaying the label and inputfield
            UIView rectangle3 = new UIView(new CGRect(0, 190, View.Bounds.Width, 50));
            rectangle3.BackgroundColor = UIColor.White;
            View.AddSubview(rectangle3);

            // Label
            UILabel newPort = new UILabel();
            newPort.Text = "New Port:";
            newPort.Frame = new CGRect(15, 10, 100, 31);
            rectangle3.AddSubview(newPort);

            // Inputfield
            UITextField changePortField = new UITextField();
            changePortField.Frame = new CGRect(110, 10, View.Bounds.Width - 125, 31);
            changePortField.Placeholder = "Introduce a port";
            rectangle3.AddSubview(changePortField);

            //View.BackgroundColor = Globals.DefaultLightGray;
            Title = "New server";

            // Save button
            done = new UIBarButtonItem(UIBarButtonSystemItem.Done, (s, e) => {
                matchesName = rxName.Matches(changeNameField.Text);
                matchesIP = rxIP.Matches(changeIPField.Text);

                if(matchesName.Count<=0)
                {
                    var okAlertController = UIAlertController.Create("Error", "You must give a name to the server", UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(okAlertController, true, null);
                }
                else if(matchesIP.Count<=0)
                {
                    var okAlertController = UIAlertController.Create("Error", "X.X.X.X'. X should be between 0 or 255", UIAlertControllerStyle.Alert);
                    okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(okAlertController, true, null);
                }
                else{
                    changeIPField.Text = "https://" + changeIPField.Text;
                    serverToAdd.AddServer(changeNameField.Text, changeIPField.Text, int.Parse(changePortField.Text));

                    ViewControllerServerList add = this.Storyboard.InstantiateViewController("ServerList") as ViewControllerServerList;
                    this.NavigationController.PushViewController(add, true);
                }
            });
            NavigationItem.RightBarButtonItem = done;
		}
	}
}