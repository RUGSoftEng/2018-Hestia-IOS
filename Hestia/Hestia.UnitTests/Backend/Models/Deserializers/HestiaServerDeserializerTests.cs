using System;
using System.Collections.Generic;
using Hestia.Backend;
using Hestia.Backend.Models;
using Hestia.Backend.Models.Deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Hestia.UnitTests.Backend.Models.Deserializers
{
    [TestClass]
    public class HestiaServerDeserializerTests
    {
        private HestiaServerDeserializer deserializer;
        private NetworkHandler networkHandler;
        private JObject jsonServer;
        
        [TestInitialize]
        public void Setup()
        {
            deserializer = new HestiaServerDeserializer();
            jsonServer = new JObject
            {
                ["created_at"] = "dummyTime",
                ["server_id"] = "dummyId",
                ["server_name"] = "dummyName",
                ["server_address"] = "https://dummyAddress",
                ["server_port"] = "1000",
                ["updated_at"] = "dummyType",
                ["user_id"] = "dummyId"
            };
            networkHandler = new NetworkHandler("https://dummyAddress:1000", "dummyToken");

            Assert.IsNotNull(deserializer);
            Assert.IsNotNull(jsonServer);
        }

        [TestMethod]
        public void DeserializeServerTest()
        {
            HestiaServer server = deserializer.DeserializeServer(jsonServer, networkHandler);

            Assert.AreEqual("dummyId", server.Id);
            Assert.AreEqual("dummyName", server.Name);
            Assert.AreEqual("https://dummyAddress", server.Address);
            Assert.AreEqual(1000, server.Port);
            Assert.AreEqual(false, server.Selected);
            Assert.AreEqual(networkHandler, server.Interactor.NetworkHandler);
        }

        [TestMethod]
        public void DeserializeServersTest()
        {
            JArray jsonServers = new JArray
            {
                jsonServer
            };
            List<HestiaServer> servers = deserializer.DeserializeServers(jsonServers, networkHandler);

            Assert.AreEqual("dummyId", servers[0].Id);
            Assert.AreEqual("dummyName", servers[0].Name);
            Assert.AreEqual("https://dummyAddress", servers[0].Address);
            Assert.AreEqual(1000, servers[0].Port);
            Assert.AreEqual(false, servers[0].Selected);
            Assert.AreEqual(networkHandler, servers[0].Interactor.NetworkHandler);
        }
    }
}
