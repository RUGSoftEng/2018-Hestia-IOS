using System;
using System.Collections.Generic;
using UIKit;
using Foundation;

using System.Drawing;
using System.Collections;

namespace Hestia.DevicesScreen
{
    public class MyEventArgs : EventArgs
    {
        public NSIndexPath indexPath { get; set; }

        public bool SwitchState { get; set; }

        public MyEventArgs() : base()
        {
        }
    }
}
