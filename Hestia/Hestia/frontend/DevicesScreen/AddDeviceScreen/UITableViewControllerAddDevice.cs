using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.backend.models;

namespace Hestia
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
                Console.WriteLine(ex.ToString());
            }
            
            // Contains methods that describe behavior of table
            TableView.Source = new TableSourceAddDeviceManufacturer(collections, collection_plugins, this);
        }
    }
}