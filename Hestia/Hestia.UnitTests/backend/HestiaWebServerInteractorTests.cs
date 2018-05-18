﻿using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hestia.UnitTests.backend
{
    [TestClass]
    public class HestiaWebServerInteractorTests
    {
        private NetworkHandler networkHandler;
        private HestiaWebServerInteractor serverInteractor;
        private const string IP = "dummyIp";
        private const int PORT = 1000;
        private const string TOKEN = "dummyToken";

        [TestInitialize]
        public void SetUpHestiaWebServerInteractor()
        {
            networkHandler = new NetworkHandler(IP, PORT, TOKEN);
            serverInteractor = new HestiaWebServerInteractor(networkHandler);

            Assert.IsNotNull(serverInteractor);
        }

        [TestMethod]
        public void SetAndGetNetworkHandlerTest()
        {
            Assert.AreEqual(networkHandler, serverInteractor.NetworkHandler);

            string newIp = "1.0.0.0";
            int newPort = 2000;
            NetworkHandler newNetworkHandler = new NetworkHandler(newIp, newPort);

            Assert.AreNotEqual(newNetworkHandler, networkHandler);

            serverInteractor.NetworkHandler = newNetworkHandler;

            Assert.AreEqual(newNetworkHandler, serverInteractor.NetworkHandler);
        }

        [TestMethod]
        public void PostUserTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(IP, PORT, TOKEN);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            serverInteractor.NetworkHandler = mockNetworkHandler.Object;

            try
            {
                serverInteractor.PostUser();
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void PostUserTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(IP, PORT, TOKEN);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Throws(new ServerInteractionException());
            serverInteractor.NetworkHandler = mockNetworkHandler.Object;

            serverInteractor.PostUser();
        }

        [TestMethod]
        public void GetServerAndServersTestSuccess()
        {
            // GetServer()
            JObject serverJson = new JObject
            {
                ["created_at"] = "dummyTime",
                ["server_id"] = "dummyId",
                ["server_name"] = "dummyName",
                ["server_port"] = "dummyPort",
                ["updated_at"] = "dummyType",
                ["user_id"] = "dummyId"
            };

            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(IP, PORT, TOKEN);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Returns(serverJson);
            serverInteractor.NetworkHandler = mockNetworkHandler.Object;

            HestiaServer server = null;
            try
            {
                server = serverInteractor.GetServer("dummyId");
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(server);
            Assert.AreEqual("dummyId", server.Id);
            Assert.AreEqual("dummyName", server.Name);
            Assert.IsNotNull(server.Interactor);
            Assert.AreEqual(IP, server.Interactor.NetworkHandler.Ip);
            Assert.AreEqual(PORT, server.Interactor.NetworkHandler.Port);

            // GetServers()
            JArray serversJson = new JArray
            {
                serverJson
            };

            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Returns(serversJson);

            List<HestiaServer> servers = null;
            try
            {
                servers = serverInteractor.GetServers();
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(servers);
            Assert.IsTrue(servers.Count == 1);
            Assert.AreEqual("dummyId", servers[0].Id);
            Assert.AreEqual("dummyName", servers[0].Name);
            Assert.IsNotNull(servers[0].Interactor);
            Assert.AreEqual(IP, servers[0].Interactor.NetworkHandler.Ip);
            Assert.AreEqual(PORT, servers[0].Interactor.NetworkHandler.Port);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetServerTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(IP, PORT, TOKEN);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            serverInteractor.NetworkHandler = mockNetworkHandler.Object;

            HestiaServer server = serverInteractor.GetServer("dummyId");
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetServersTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(IP, PORT, TOKEN);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            serverInteractor.NetworkHandler = mockNetworkHandler.Object;

            List<HestiaServer> servers = serverInteractor.GetServers();
        }

        [TestMethod]
        public void AddServerTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(IP, PORT, TOKEN);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            serverInteractor.NetworkHandler = mockNetworkHandler.Object;

            try
            {
                serverInteractor.AddServer("dummyName", "dummyAddress", 1000);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void AddServerTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(IP, PORT, TOKEN);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Throws(new ServerInteractionException());
            serverInteractor.NetworkHandler = mockNetworkHandler.Object;

            serverInteractor.AddServer("dummyName", "dummyAddress", 1000);
        }
    }
}