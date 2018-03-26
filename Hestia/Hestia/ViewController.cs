using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Hestia.backend;
using Hestia.backend.models;
using Newtonsoft.Json.Linq;
using UIKit;

namespace Hestia
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}