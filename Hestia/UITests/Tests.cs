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
            app.Screenshot("First screen.");
            app.Tap(b => b.Button("Local"));
            app.EnterText("Hestia");
            app.Tap(t => t.Text("Enter IP"));
            app.EnterText("https://94.212.164.28");
            app.Tap(t => t.Text("Enter port"));
            app.EnterText("8000");
            app.Tap(t => t.Text("Connect"));
            app.Tap(b => b.Button("Edit"));
            app.Tap(t => t.Text("New Device"));
            app.Tap(t => t.Text("mock"));
            app.Tap(t => t.Text("lock"));
            var l = app.Query(t => t.Text("NAME"));
            app.TapCoordinates(l[1].Rect.CenterX, l[1].Rect.CenterY + 20);
            app.EnterText("UITestLock");
            app.Tap(b => b.Button("Save"));
            app.Tap(t => t.Text("UITestLock"));
            app.Tap(c => c.Class("UISwitch"));
            app.Tap(t => t.Text("Devices"));
            app.Tap(b => b.Button("Edit"));
            //app.Tap(t => t.Text("UITestLock"));
            l = app.Query(t => t.Text("UITestLock"));
            app.TapCoordinates(l[0].Rect.X, l[0].Rect.CenterY);
            app.TapCoordinates(l[0].Rect.Width-1, l[0].Rect.CenterY);
            app.Tap(b => b.Button("Done"));
        }

        [Test]
        public void SettingsScreen()
        {
            app.Screenshot("First screen.");
            app.Tap(b => b.Button("Local"));
            app.EnterText("Hestia");
            app.Tap(t => t.Text("Enter IP"));
            app.EnterText("https://94.212.164.28");
            app.Tap(t => t.Text("Enter port"));
            app.EnterText("8000");
            app.Tap(t => t.Text("Connect"));
            app.Tap(b => b.Button("Settings"));
            app.Tap(t => t.Text("Contact Information"));
            app.Tap(b => b.Button("Settings"));
            app.Tap(t => t.Text("Server"));
            app.Tap(b => b.Button("Settings"));
        }

        [Test]
        public void GlobalLaunch()
        {
            app.Tap(e => e.Button("Global"));
        }
    }
}
