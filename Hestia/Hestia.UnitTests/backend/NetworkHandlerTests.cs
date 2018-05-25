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
        private const string ADDRESS = "https://dummyAddress";
        private const int PORT = 1000;
        private const string TOKEN = "dummyToken";

        [TestInitialize]
        public void SetUpNetworkHandler()
        {
            networkHandlerIp = new NetworkHandler(ADDRESS);
            networkHandlerIpPort = new NetworkHandler(ADDRESS, PORT);
            networkHandlerIpToken = new NetworkHandler(ADDRESS, TOKEN);
            networkHandlerIpPortToken = new NetworkHandler(ADDRESS, PORT, TOKEN);

            Assert.IsNotNull(networkHandlerIp);
            Assert.IsNotNull(networkHandlerIpPort);
            Assert.IsNotNull(networkHandlerIpToken);
            Assert.IsNotNull(networkHandlerIpPortToken);
        }

        [TestMethod]
        public void SetAndGetIpTest()
        {
            Assert.AreEqual(ADDRESS, networkHandlerIp.Address);
            Assert.AreEqual(ADDRESS, networkHandlerIpPort.Address);
            Assert.AreEqual(ADDRESS, networkHandlerIpToken.Address);
            Assert.AreEqual(ADDRESS, networkHandlerIpPortToken.Address);

            string newAddress = "https://1.0.0.0";
            networkHandlerIp.Address = newAddress;
            networkHandlerIpPort.Address = newAddress;
            networkHandlerIpToken.Address = newAddress;
            networkHandlerIpPortToken.Address = newAddress;

            Assert.AreEqual(newAddress, networkHandlerIp.Address);
            Assert.AreEqual(newAddress, networkHandlerIpPort.Address);
            Assert.AreEqual(newAddress, networkHandlerIpToken.Address);
            Assert.AreEqual(newAddress, networkHandlerIpPortToken.Address);
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
