using System;
using System.Collections.Generic;
using UIKit;
using System.Collections;
using Hestia.Backend.Exceptions;
using Hestia.Frontend.Resources;

namespace Hestia.Frontend.DevicesScreen.AddDeviceScreen
{
    public partial class UITableViewControllerAddDevice : UITableViewController
    {
        // List of manufacturers
        List<string> collections;
        
        // Stores list of plugins per manufacturer
        Hashtable collection_plugins;

        public UITableViewControllerAddDevice (IntPtr handle) : base (handle)
        {
            collection_plugins = new Hashtable();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // The list with manufacturers
            collections = new List<string>();
            try
            {
                collections = Globals.ServerToAddDeviceTo.GetCollections();
                foreach(string collection in collections)
                {
                    // Get plugins for this collection
                    List<string> plugins = Globals.ServerToAddDeviceTo.GetPlugins(collection);
                    // Add this collection/plugins combination to hashtable
                    collection_plugins.Add(collection, plugins);
                }
            }
            catch (ServerInteractionException ex)
            {
                Console.WriteLine("Exception while using serverInteractor");
                Console.WriteLine(ex);
                WarningMessage.Display("Exception", "An exception occured on the server trying to get available plugins", this);
            }
            
            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceAddDeviceManufacturer(collections, collection_plugins, this);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            // Cancel button to go back to Devices main screen
            UIBarButtonItem cancel = new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, eventArguments) =>
            {
                NavigationController.PopToRootViewController(true);
            });
            NavigationItem.RightBarButtonItem = cancel;
        }
    }
}
