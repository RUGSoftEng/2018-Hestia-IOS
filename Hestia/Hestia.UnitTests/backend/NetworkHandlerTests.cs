using System;
using Hestia.backend;
using Hestia.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Hestia.UnitTests.backend
{
    [TestClass]
    public class NetworkHandlerTests
    {
        private NetworkHandler dummyNetworkHandler;
        private string dummyIp = "0.0.0.0";
        private int dummyPort = 1000;

        [TestInitialize]
        public void SetUpNetworkHandler()
        {
            dummyNetworkHandler = new NetworkHandler(dummyIp, dummyPort);
            Assert.IsNotNull(dummyNetworkHandler);
        }

        [TestMethod]
        public void SetAndGetIpTest()
        {
            Assert.AreEqual(dummyIp, dummyNetworkHandler.Ip);
            string newIp = "1.0.0.0";
            dummyNetworkHandler.Ip = newIp;
            Assert.AreEqual(newIp, dummyNetworkHandler.Ip);
        }

        [TestMethod]
        public void SetAndGetPortTest()
        {
            Assert.AreEqual(dummyPort, dummyNetworkHandler.Port);
            int newPort = 2000;
            dummyNetworkHandler.Port = newPort;
            Assert.AreEqual(newPort, dummyNetworkHandler.Port);
        }
    }
}
