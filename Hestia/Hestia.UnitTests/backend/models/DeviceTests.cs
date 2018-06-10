using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.UnitTests.backend.models
{
    [TestClass]
    public class DeviceTests
    {
        private Device device;
        private string deviceId = "123";
        private string deviceName = "dummyName";
        private string type = "dummytype";
        private List<Activator> activators;
        private Activator activator;
        private string dummyAddress = "https://1.1.1.1:1000";
        private NetworkHandler networkHandler;
        private HestiaServerInteractor serverInteractor;

        [TestInitialize]
        public void SetUpDevice()
        {
            ActivatorState activatorState = new ActivatorState(false, "bool");
            string activatorId = "1234";
            string activatorName = "bob";
            int activatorRank = 0;
            activator = new Activator(activatorId, activatorName, activatorRank, activatorState);

            activators = new List<Activator>
            {
                activator
            };

            networkHandler = new NetworkHandler(dummyAddress);
            serverInteractor = new HestiaServerInteractor(networkHandler);

            device = new Device(deviceId, deviceName, type, activators, serverInteractor);

            activator.Device = device;

            Assert.IsNotNull(device);
        }

        [TestMethod]
        public void SetAndGetIdTest()
        {
            Assert.AreEqual(deviceId, device.DeviceId);
            string newId = "1234";
            device.DeviceId = newId;
            Assert.AreEqual(newId, device.DeviceId);
        }

        [TestMethod]
        public void SetAndGetNameTestSuccess()
        {
            Assert.AreEqual(deviceName, device.Name);

            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(dummyAddress);
            mockNetworkHandler.Setup(x => x.Put(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            device.ServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string newName = "newName";
            try
            {
                device.Name = newName;
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.AreEqual(newName, device.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void SetNameTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(dummyAddress);
            mockNetworkHandler.Setup(x => x.Put(It.IsAny<JObject>(), It.IsAny<string>())).Throws(new ServerInteractionException());
            device.ServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string newName = "amazingName";
            device.Name = newName;
        }

        [TestMethod]
        public void SetAndGetTypeTest()
        {
            Assert.AreEqual(type, device.Type);
            string newType = "light";
            device.Type = newType;
            Assert.AreEqual(newType, device.Type);
        }

        [TestMethod]
        public void SetAndGetActivatorsTest()
        {
            Assert.IsTrue(device.Activators.Count == 1);
            Assert.AreEqual(activators, device.Activators);

            ActivatorState newActivatorState = new ActivatorState(0.8, "float");
            string newActivatorId = "1000";
            string newActivatorName = "slider";
            int newActivatorRank = 15;
            Activator newActivator = new Activator(newActivatorId, newActivatorName, newActivatorRank, newActivatorState);

            List<Activator> newActivators = new List<Activator>
            {
                newActivator
            };

            device.Activators = newActivators;

            Assert.IsTrue(device.Activators.Count == 1);
            Assert.AreEqual(newActivators, device.Activators);
        }

        [TestMethod]
        public void SetAndGetServerInteractorTest()
        {
            Assert.AreEqual(serverInteractor, device.ServerInteractor);

            string newAddress = "https://2.2.2.2:2000";
            NetworkHandler newNetworkHandler = new NetworkHandler(newAddress);
            HestiaServerInteractor newServerInteractor = new HestiaServerInteractor(newNetworkHandler);

            device.ServerInteractor = newServerInteractor;

            Assert.AreEqual(newServerInteractor, device.ServerInteractor);
        }
    }
}
