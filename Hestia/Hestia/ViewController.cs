using System;
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
                HttpClient client = new HttpClient();

                NetworkHandler networkHandler = new NetworkHandler(ip, port, client);

                var devices = networkHandler.Get("devices/");
                Console.WriteLine(devices);
            };
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.
        }
    }
}