using System;
using System.Collections.Generic;
using UIKit;
using Foundation;
using Hestia.DevicesScreen;
using Hestia.DevicesScreen.resources;
using System.Drawing;
using System.Collections;
using Hestia.backend;
using Hestia.backend.models;


namespace Hestia
{
    public partial class UITableViewControllerAddDevice : UITableViewController
    {
        // The table that lives in this view controller
        UITableView table;

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
            table = new UITableView(View.Bounds); // defaults to Plain style

            // The list with manufacturers
            try
            {
                collections = Globals.ServerInteractor.GetCollections();
                foreach(string collection in collections)
                {
                    // Get plugins for this collection
                    List<string> plugins = Globals.ServerInteractor.GetPlugins(collection);
                    // Add this collection/plugins combination to hashtable
                    collection_plugins.Add(collection, plugins);
                }
     
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception while using serverInteractor");
                Console.WriteLine(e.StackTrace);
            }
            // Contains methods that describe behavior of table
            table.Source = new TableSourceAddDeviceManufacturer(collections, collection_plugins, this);

            // Add the table to the view
            Add(table);

        }
    }
}