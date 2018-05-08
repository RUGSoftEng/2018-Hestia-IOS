using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hestia.backend;

namespace Hestia.UnitTests.backend
{
    [TestClass]
    public class WebServerTests
    {
        private WebServer dummyServer;
        private string ip = "0.0.0.0";
        private int port = 1000;

        [TestInitialize]
        public void SetUpWebServer()
        {
            dummyServer = new WebServer(false, null);
            Assert.IsNotNull(dummyServer);
        }

        [TestMethod]
        public void GetSelectedTest()
        {
            bool test = dummyServer.Selected;
            Assert.IsFalse(test);
        }

        [TestMethod]
        public void SetSelectedTest()
        {
            bool test = true;
            dummyServer.Selected = test;
            Assert.IsTrue(dummyServer.Selected);
        }

        [TestMethod]
        public void SetServerInteractor()
        {
            NetworkHandler dummyNetworkHandler = new NetworkHandler(ip, port);
            ServerInteractor dummyServerInteractor = new ServerInteractor(dummyNetworkHandler);
            dummyServer.Interactor = dummyServerInteractor;
            Assert.IsTrue(dummyServer.Interactor.Equals(dummyServerInteractor));
        }
        
        [TestMethod]
        public void GetServerInteractor()
        {
            // This test will fail because there has not been a good .Equals implemented for the interactor and handler.
            ServerInteractor testInteractor = dummyServer.Interactor;
            Assert.IsTrue(dummyServer.Interactor.Equals(testInteractor));
        }
    }
}
