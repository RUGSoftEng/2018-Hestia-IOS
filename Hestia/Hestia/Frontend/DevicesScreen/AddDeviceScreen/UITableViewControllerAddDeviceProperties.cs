﻿using Hestia.Backend.Exceptions;
using Hestia.Backend.Models;
using Hestia.Frontend.Resources;
using System;
using System.Collections;
using System.Text.RegularExpressions;
using UIKit;

namespace Hestia.Frontend.DevicesScreen.AddDeviceScreen
{
    /// <summary>
    /// This view show the required info that is needed in order
    /// to create a new device.
    /// </summary>
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
                if (matchesIP != null)
                {
                    if (matchesName.Count <= 0 && matchesIP.Count <= 0)
                    {
                        WarningMessage.Display("Error!", "You have to fill all the specifictions.", this);
                    }
                    else if (matchesName.Count <= 0 && matchesIP.Count > 0)
                    {
                        WarningMessage.Display("Error!", "You have to give a name for the device.", this);
                    }
                    else if (matchesIP.Count <= 0 && matchesName.Count > 0)
                    {
                        WarningMessage.Display("Error!", "X.X.X.X'. X should be between 0 or 255", this);
                    }
                    else
                    {
                        AddDeviceToServer();
                    }
                }
                else
                {
                    if (matchesName.Count <= 0)
                    {
                        WarningMessage.Display("Error!", "You have to give a name for the device.", this);
                    }
                    else
                    {
                        AddDeviceToServer();
                    }
                }    
            });

            // Set right button to save 
            NavigationItem.RightBarButtonItem = save;
        }

        void AddDeviceToServer()
        {
            // Try to add device to server
            try
            {
                Globals.ServerToAddDeviceTo.AddDevice(pluginInfo);
                SegueToDevicesMain();
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while adding device to server");
                Console.WriteLine(ex);
                WarningMessage.Display("Exception", "Could not add device", this);
            }
        }

        void SegueToDevicesMain()
        {
            // Get the root view contoller and cancel the editing state
            var rootViewController = NavigationController.ViewControllers[0] as UITableViewControllerDevicesMain;
            rootViewController.CancelEditingState();
            rootViewController.RefreshDeviceList();
            // Go back to the devices main screen
            NavigationController.PopToViewController(rootViewController, true);
        }
    }
}
