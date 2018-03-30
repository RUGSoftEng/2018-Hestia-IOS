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
        UITableView table;

        List<string> collections;
        Hashtable collection_plugins;
        public UITableViewControllerAddDevice (IntPtr handle) : base (handle)
        {
            collection_plugins = new Hashtable();
        }


        // The table that lives in this view controller
       

        public override void ViewDidLoad()
        {

            base.ViewDidLoad();
            table = new UITableView(View.Bounds); // defaults to Plain style

            // The list with manufacturers
            try
            {
                collections = Globals.ServerInteractor.GetCollections();
                foreach(string col in collections)
                {
                    List<string> plugins = Globals.ServerInteractor.GetPlugins(col);
                    collection_plugins.Add(col, plugins);
                }
     
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.StackTrace);
            }
            // Contains methods that describe behavior of table
            table.Source = new TableSourceAddDeviceManufacturer(collections, collection_plugins, this);

            //table.Source = new TableSource(tableItems, this); 
            // Add the table to the view
            Add(table);

        }
    }
}