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
        private NetworkHandler networkHandlerAddress;
        private NetworkHandler networkHandlerAddressToken;
        private const string ADDRESS = "https://dummyAddress:1000";
        private const string TOKEN = "dummyToken";

        [TestInitialize]
        public void SetUpNetworkHandler()
        {
            networkHandlerAddress = new NetworkHandler(ADDRESS);
            networkHandlerAddressToken = new NetworkHandler(ADDRESS, TOKEN);

            Assert.IsNotNull(networkHandlerAddress);
            Assert.IsNotNull(networkHandlerAddressToken);
        }

        [TestMethod]
        public void SetAndGetAddressTest()
        {
            Assert.AreEqual(ADDRESS, networkHandlerAddress.Address);
            Assert.AreEqual(ADDRESS, networkHandlerAddressToken.Address);

            string newAddress = "https://1.0.0.0:2000";
            networkHandlerAddress.Address = newAddress;
            networkHandlerAddressToken.Address = newAddress;

            Assert.AreEqual(newAddress, networkHandlerAddress.Address);
            Assert.AreEqual(newAddress, networkHandlerAddressToken.Address);
        }
    }
}
