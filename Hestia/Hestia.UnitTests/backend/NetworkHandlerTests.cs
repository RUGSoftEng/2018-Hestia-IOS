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
        private NetworkHandler networkHandlerIp;
        private NetworkHandler networkHandlerIpPort;
        private NetworkHandler networkHandlerIpToken;
        private NetworkHandler networkHandlerIpPortToken;
        private const string IP = "dummyIp";
        private const int PORT = 1000;
        private const string TOKEN = "dummyToken";

        [TestInitialize]
        public void SetUpNetworkHandler()
        {
            networkHandlerIp = new NetworkHandler(IP);
            networkHandlerIpPort = new NetworkHandler(IP, PORT);
            networkHandlerIpToken = new NetworkHandler(IP, TOKEN);
            networkHandlerIpPortToken = new NetworkHandler(IP, PORT, TOKEN);

            Assert.IsNotNull(networkHandlerIp);
            Assert.IsNotNull(networkHandlerIpPort);
            Assert.IsNotNull(networkHandlerIpToken);
            Assert.IsNotNull(networkHandlerIpPortToken);
        }

        [TestMethod]
        public void SetAndGetIpTest()
        {
            Assert.AreEqual(IP, networkHandlerIp.Address);
            Assert.AreEqual(IP, networkHandlerIpPort.Address);
            Assert.AreEqual(IP, networkHandlerIpToken.Address);
            Assert.AreEqual(IP, networkHandlerIpPortToken.Address);

            string newIp = "1.0.0.0";
            networkHandlerIp.Address = newIp;
            networkHandlerIpPort.Address = newIp;
            networkHandlerIpToken.Address = newIp;
            networkHandlerIpPortToken.Address = newIp;

            Assert.AreEqual(newIp, networkHandlerIp.Address);
            Assert.AreEqual(newIp, networkHandlerIpPort.Address);
            Assert.AreEqual(newIp, networkHandlerIpToken.Address);
            Assert.AreEqual(newIp, networkHandlerIpPortToken.Address);
        }

        [TestMethod]
        public void SetAndGetPortTest()
        {
            Assert.AreEqual(PORT, networkHandlerIpPort.Port);
            Assert.AreEqual(PORT, networkHandlerIpPortToken.Port);

            int newPort = 2000;
            networkHandlerIpPort.Port = newPort;
            networkHandlerIpPortToken.Port = newPort;

            Assert.AreEqual(newPort, networkHandlerIpPort.Port);
            Assert.AreEqual(newPort, networkHandlerIpPortToken.Port);
        }

        [TestMethod]
        public void UsesAuthTest()
        {
            Assert.AreEqual(false, networkHandlerIp.UsesAuth);
            Assert.AreEqual(false, networkHandlerIpPort.UsesAuth);
            Assert.AreEqual(true, networkHandlerIpToken.UsesAuth);
            Assert.AreEqual(true, networkHandlerIpPortToken.UsesAuth);
        }

        [TestMethod]
        public void HasPortTest()
        {
            Assert.AreEqual(false, networkHandlerIp.HasPort);
            Assert.AreEqual(true, networkHandlerIpPort.HasPort);
            Assert.AreEqual(false, networkHandlerIpToken.HasPort);
            Assert.AreEqual(true, networkHandlerIpPortToken.HasPort);
        }
    }
}
