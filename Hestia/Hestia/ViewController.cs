using System;
using System.Net;
using System.Net.Http;
using Hestia.backend;

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

            string ip;
            int port;

            ConnectButton.TouchUpInside += (object sender, EventArgs e) =>
            {
                ip = ServerIp.Text;
                port = Int32.Parse(ServerPort.Text);

                NetworkHandler networkHandler = new NetworkHandler(ip, port);

                string devices = networkHandler.Get("devices");
                Console.WriteLine(devices);

                string renameDevice =
                "{ \"name\": \"test2\" }";
                string response = networkHandler.Put(renameDevice, "devices/5ab37fcde82b3f07245b9d39");
                Console.WriteLine(response);
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}