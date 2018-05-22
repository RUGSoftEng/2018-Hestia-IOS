using System;
using System.Collections.Generic;
using Hestia.backend;
using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Hestia.UnitTests.backend.models.deserializers
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
                ["server_port"] = "dummyPort",
                ["updated_at"] = "dummyType",
                ["user_id"] = "dummyId"
            };
            networkHandler = new NetworkHandler("dummyIp", 1000, "dummyToken");

            Assert.IsNotNull(deserializer);
            Assert.IsNotNull(jsonServer);
        }

        [TestMethod]
        public void DeserializeServerTest()
        {
            HestiaServer server = deserializer.DeserializeServer(jsonServer, networkHandler);

            Assert.AreEqual("dummyId", server.Id);
            Assert.AreEqual("dummyName", server.Name);
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
            Assert.AreEqual(false, servers[0].Selected);
            Assert.AreEqual(networkHandler, servers[0].Interactor.NetworkHandler);
        }
    }
}
