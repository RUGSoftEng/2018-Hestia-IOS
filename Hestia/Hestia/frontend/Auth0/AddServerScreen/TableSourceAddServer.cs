using System;
using UIKit;
using Foundation;
using System.Collections;
using Hestia.DevicesScreen.resources;
using Hestia.Auth0;
using System.Collections.Generic;

using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.frontend;

namespace Hestia
{
    public class TableSourceAddServer : UITableViewSource
    {
        public Hashtable inputs;
        public List<string> requiredinfo;
        AddServerViewController owner;

        public TableSourceAddServer(AddServerViewController owner)
        {
            this.owner = owner;
            inputs = owner.inputFields;
            List<string> aux = new List<string>();
            aux.Add("name");
            aux.Add("address");
            aux.Add("port");
            requiredinfo = aux;

        }

		public override nint NumberOfSections(UITableView tableView)
		{
            return requiredinfo.Count;
		}

		public override nint RowsInSection(UITableView tableview, nint section)
		{
            return 1;
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            // request a recycled cell to save memory
            PropertyCell cell = tableView.DequeueReusableCell(Resources.strings.propertyCell) as PropertyCell;
            // if there are no cells to reuse, create a new one
            if (cell == null)
            {
                // Generate a default propertyCell
                cell = new PropertyCell((NSString)Resources.strings.propertyCell);
            }

            inputs[requiredinfo[indexPath.Row]] = cell;
            return cell;
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
            return requiredinfo[(int)section];
		}
	}
}
