using System;
using System.Collections;
using UIKit;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.AddDeviceScreen;
using Hestia.DevicesScreen.resources;
using Hestia.frontend;
using System.Text.RegularExpressions;

namespace Hestia
{
    public partial class UITableViewControllerAddDeviceProperties : UITableViewController
    {
        // Plugin info set at creation in devices window
        public PluginInfo pluginInfo;
        // Keeps track at the input fields for device properties
        public Hashtable inputFields = new Hashtable();

        Regex rxIP = new Regex( @"((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.|$)){4}");
        Regex rxName = new Regex(@"^(.)+$");
        MatchCollection matchesName, matchesIP;


        public UITableViewControllerAddDeviceProperties(IntPtr handle) : base(handle)
        {
        }

        // Called on press of the save button. It copies the values that are input 
        // in the text fields to the pluginInfo object
        public void SaveFields()
        {
            // Used for loop through original property names
            string[] propertyNames = new string[pluginInfo.RequiredInfo.Keys.Count];
            pluginInfo.RequiredInfo.Keys.CopyTo(propertyNames, 0);

            foreach (string property in propertyNames)
            {
                if(property.Equals("name")){
                    matchesName = rxName.Matches(((PropertyCell)inputFields[property]).inputField.Text);
                }
                if(property.Equals("ip")){
                    matchesIP = rxIP.Matches(((PropertyCell)inputFields[property]).inputField.Text);
                }

                pluginInfo.RequiredInfo[property] = ((PropertyCell)inputFields[property]).inputField.Text;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceAddDeviceProperties(this);
            View.BackgroundColor = Globals.DefaultLightGray;
            TableView.AllowsSelection = false;
            // Save button
            UIBarButtonItem save = new UIBarButtonItem(UIBarButtonSystemItem.Save, (sender, eventArguments) => {
                SaveFields();
                Console.WriteLine("Clicked save button");

                if (matchesName.Count <= 0 && matchesIP.Count <= 0)
                {
                    var alert = UIAlertController.Create("Error!", "You have to fill all the specifictions.", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                else if (matchesName.Count <= 0 && matchesIP.Count > 0)
                {
                    var alert = UIAlertController.Create("Error!", "You have to give a name for the device.", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                else if (matchesIP.Count <= 0 && matchesName.Count > 0)
                {
                    var alert = UIAlertController.Create("Error!", "IP= 'X.X.X.X'. X should be between 0 or 255", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    PresentViewController(alert, true, null);
                }
                else
                {
                    // Try to add device to server
                    try
                    {
                        Console.WriteLine("Server to add device to" + Globals.ServerToAddDeviceTo);
                        Globals.ServerToAddDeviceTo.AddDevice(pluginInfo);

                    }
                    catch (ServerInteractionException ex)
                    {
                        Console.WriteLine("Exception while adding device to server");
                        Console.WriteLine(ex);
                        frontend.WarningMessage message = new frontend.WarningMessage("Exception", "Could not add device", this);
                    }
                }

                // Get the root view contoller and cancel the editing state
                var rootViewController = NavigationController.ViewControllers[0] as UITableViewControllerDevicesMain;
                rootViewController.CancelEditingState();
                rootViewController.RefreshDeviceList();
                // Go back to the devices main screen
                NavigationController.PopToViewController(rootViewController, true);
            });

            // Set right button to save 
            NavigationItem.RightBarButtonItem = save;
        }
    }
}
