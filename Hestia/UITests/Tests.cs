using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

namespace UITests
{
    [TestFixture]
    public class Tests
    {
        iOSApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            app = ConfigureApp
                .iOS
                .StartApp();
        }

        [Test]
        public void DevicesScreen()
        {
            app.Screenshot("Login");
            app.Tap(b => b.Button("Local"));
            app.EnterText("Hestia");
            app.Screenshot("Server connect");
            app.Tap(t => t.Text("Enter IP"));
            app.EnterText("https://94.212.164.28");
            app.Tap(t => t.Text("Enter port"));
            app.EnterText("8000");
            app.Tap(t => t.Text("Connect"));
            app.Screenshot("Devices");
            app.Tap(b => b.Button("Edit"));
            app.Screenshot("Devices edit mode");
            app.Tap(t => t.Text("New Device"));
            app.Screenshot("New device manufacturer");
            app.Tap(t => t.Text("mock"));
            app.Screenshot("New device type");
            app.Tap(t => t.Text("lock"));
            app.Screenshot("New device properties");
            var l = app.Query(t => t.Text("NAME"));
            app.TapCoordinates(l[1].Rect.CenterX, l[1].Rect.CenterY + 20);
            app.EnterText("UITestLock");
            app.Tap(b => b.Button("Save"));
            app.Screenshot("Added device");
            app.Tap(t => t.Text("UITestLock"));
            app.Screenshot("Device activators");
            app.Tap(c => c.Class("UISwitch"));
            app.Tap(t => t.Text("Devices"));
            app.Tap(b => b.Button("Edit"));
            l = app.Query(t => t.Text("UITestLock"));
            app.TapCoordinates(l[0].Rect.X, l[0].Rect.CenterY);
            app.Screenshot("Delete device");
            app.TapCoordinates(l[0].Rect.Width-1, l[0].Rect.CenterY);
            app.Tap(b => b.Button("Done"));
        }

        [Test]
        public void addPhilipsDevice()
        {
            app.Tap(b => b.Button("Local"));
            app.EnterText("Hestia");
            app.Tap(t => t.Text("Enter IP"));
            app.EnterText("https://94.212.164.28");
            app.Tap(t => t.Text("Enter port"));
            app.EnterText("8000");
            app.Tap(t => t.Text("Connect"));
            app.Tap(b => b.Button("Edit"));
            app.Tap(t => t.Text("New Device"));
            app.Tap(t => t.Text("philips"));
            app.Tap(t => t.Text("colorLight"));

            var l = app.Query(t => t.Text("NAME"));
            app.TapCoordinates(l[1].Rect.CenterX, l[1].Rect.CenterY + 20);
            app.EnterText("Test");


            l = app.Query(t => t.Text("BRIDGE_IP"));
            app.TapCoordinates(l[1].Rect.CenterX, l[1].Rect.CenterY + 20);
            app.EnterText("1");


            app.Tap(b => b.Button("Save"));
        }

        [Test]
        public void SettingsScreens()
        {
            app.Tap(b => b.Button("Local"));
            app.EnterText("Hestia");
            app.Tap(t => t.Text("Enter IP"));
            app.EnterText("https://94.212.164.28");
            app.Tap(t => t.Text("Enter port"));
            app.EnterText("8000");
            app.Tap(t => t.Text("Connect"));
            app.Tap(b => b.Button("Settings"));
            app.Screenshot("Settings");
            app.Tap(t => t.Text("Contact Information"));
            app.Screenshot("Contact");
            app.Tap(b => b.Button("Settings"));
            app.Tap(t => t.Text("Server"));
            app.Screenshot("Server settings");
            app.Tap(b => b.Button("Settings"));
        }

        [Test]
        public void GlobalLaunch()
        {
            app.Tap(e => e.Button("Global"));
        }
    }
}
