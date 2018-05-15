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
        public void AppLaunches()
        {
            app.Screenshot("First screen.");
            app.Tap(e => e.Button("Local"));
            app.EnterText("Hestia");
            app.Tap(e => e.Text("Enter IP"));
            app.EnterText("94.212.164.28");
            app.Tap(e => e.Text("Enter port"));
            app.EnterText("8000");
            //app.Repl();
        }

        [Test]
        public void AppConnect()
        {
            app.Tap(e => e.Button("Global"));
        }
    }
}
