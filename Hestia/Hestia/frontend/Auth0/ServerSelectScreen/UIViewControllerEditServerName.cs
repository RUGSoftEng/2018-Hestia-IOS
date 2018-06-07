using System;

using System;
using UIKit;
using CoreGraphics;

using Hestia.DevicesScreen.resources;
using Hestia.backend.models;
using Hestia.backend.exceptions;
using Hestia.frontend;
namespace Hestia.frontend.Auth0.ServerSelectScreen
{
    public class UIViewControllerEditServerName : UIViewController
    {

        ViewControllerServerList owner;
        public HestiaServer server;
        UIBarButtonItem saveName;

        public UIViewControllerEditServerName(ViewControllerServerList owner)
        {
            this.owner = owner;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

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
            changeNameField.Placeholder = server.Name;
            rectangle.AddSubview(changeNameField);

            View.BackgroundColor = Globals.DefaultLightGray;
            Title = server.Name;

            // Save button
            saveName = new UIBarButtonItem(UIBarButtonSystemItem.Save, (s, e) => {
                if (changeNameField.Text.Length <= 0)
                {
                    WarningMessage.Display("Error", "You have to give a name for the device.", this);
                }
                else
                {
                    try
                    {
                        //server.Name = changeNameField.Text;
                        Globals.HestiaWebServerInteractor.EditServer(server, changeNameField.Text, server.Address, server.Port);
                        // Reset editing mode to be able to correctly update cell contents
                        owner.CancelEditingState();

                        owner.RefreshServerList();
                        owner.SetEditingState();
                        NavigationController.PopViewController(true);
                    }
                    catch (ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while changing device name");
                        Console.WriteLine(ex);
                        WarningMessage.Display("Exception", "An exception occurred on the server when changing the name of the device", this);
                    }
                }
            });
            NavigationItem.RightBarButtonItem = saveName;
        }
    }

}



