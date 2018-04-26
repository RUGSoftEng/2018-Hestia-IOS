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
using Hestia.backend.utils;

namespace Hestia
{
    public partial class UITableViewServerList : UITableViewController
    {
        
        List<FireBaseServer> serverList = new List<FireBaseServer>()
        {
            new FireBaseServer(false, new ServerInteractor(new NetworkHandler("94.212.164.28", 8000))),
            new FireBaseServer(false, new ServerInteractor(new NetworkHandler("94.212.164.26", 8000))),
            new FireBaseServer(false, new ServerInteractor(new NetworkHandler("94.212.164.27", 8000))),
        };

        public UITableViewServerList (IntPtr handle) : base (handle)
        {
        }

		public override void ViewDidLoad()
		{
            base.ViewDidLoad();
            Globals.LocalLogin = false;
            TableView.Source = new TableSourceServerList(serverList, this);

		}



	}
}