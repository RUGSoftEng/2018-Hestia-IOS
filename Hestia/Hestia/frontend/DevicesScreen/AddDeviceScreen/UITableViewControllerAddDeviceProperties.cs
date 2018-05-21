using System;
using System.Collections;
using UIKit;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.AddDeviceScreen;
using Hestia.DevicesScreen.resources;

namespace Hestia
{
    public partial class UITableViewControllerAddDeviceProperties : UITableViewController
    {
        // Plugin info set at creation in devices window
        public PluginInfo pluginInfo;
        // Keeps track at the input fields for device properties
        public Hashtable inputFields = new Hashtable();
       
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
                pluginInfo.RequiredInfo[property] = ((PropertyCell)inputFields[property]).inputField.Text;
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceAddDeviceProperties(this);
            View.BackgroundColor = Globals.DefaultLightGray;
            // Save button
            UIBarButtonItem save = new UIBarButtonItem(UIBarButtonSystemItem.Save, (sender, eventArguments) => {
                SaveFields();

                // Try to add device to server
                try
                {
                    Globals.ServerToAddDeviceTo.AddDevice(pluginInfo);
                }
                catch (ServerInteractionException ex)
                {
                    Console.WriteLine("Exception while adding device to server");
                    Console.WriteLine(ex.ToString());
                }

                // Get the root view contoller and cancel the editing state
                var rootViewController = this.NavigationController.ViewControllers[0] as UITableViewControllerDevicesMain;
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
