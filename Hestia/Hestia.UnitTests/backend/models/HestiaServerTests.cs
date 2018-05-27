﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Hestia.backend;
using Hestia.backend.models;

namespace Hestia.UnitTests.backend
{
    [TestClass]
    public class HestiaServerTests
    {
        private HestiaServer dummyServer;
        private string address = "https://0.0.0.0";
        private int port = 1000;

        [TestInitialize]
        public void SetUpWebServer()
        {
            dummyServer = new HestiaServer(false, null);
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
            NetworkHandler dummyNetworkHandler = new NetworkHandler(address, port);
            HestiaServerInteractor dummyServerInteractor = new HestiaServerInteractor(dummyNetworkHandler);
            dummyServer.Interactor = dummyServerInteractor;
            Assert.IsTrue(dummyServer.Interactor.Equals(dummyServerInteractor));
        }

        [TestMethod]
        public void GetServerInteractor()
        {
            NetworkHandler dummyNetworkHandler = new NetworkHandler(address, port);
            HestiaServerInteractor dummyServerInteractor = new HestiaServerInteractor(dummyNetworkHandler);
            dummyServer.Interactor = dummyServerInteractor;
            HestiaServerInteractor testInteractor = dummyServer.Interactor;
            Assert.IsTrue(testInteractor.NetworkHandler.Address == dummyServer.Interactor.NetworkHandler.Address);
            Assert.IsTrue(testInteractor.NetworkHandler.Port == dummyServer.Interactor.NetworkHandler.Port);
        }
    }
}