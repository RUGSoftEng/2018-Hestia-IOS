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
    public class ActivatorTests
    {
        private string boolId;
        private string boolName;
        private int boolRank;
        private bool boolRawState;
        private string boolStateType;
        private ActivatorState boolActivatorState;
        private Activator boolActivator;

        private string floatId;
        private string floatName;
        private int floatRank;
        private float floatRawState;
        private string floatStateType;
        private ActivatorState floatActivatorState;
        private Activator floatActivator;

        private string dummyAddress = "https://0.0.0.0";
        private int dummyPort = 1000;
        private NetworkHandler networkHandler;
        private HestiaServerInteractor serverInteractor;
        private Device device;

        [TestInitialize]
        public void SetUpActivators()
        {
            networkHandler = new NetworkHandler(dummyAddress, dummyPort);
            serverInteractor = new HestiaServerInteractor(networkHandler);
            device = new Device("1234", "nice_device", "light", new List<Activator>(), serverInteractor);

            boolId = "123";
            boolName = "dummyBoolName";
            boolRank = 0;
            boolRawState = false;
            boolStateType = "bool";
            boolActivatorState = new ActivatorState(boolRawState, boolStateType);
            boolActivator = new Activator(boolId, boolName, boolRank, boolActivatorState)
            {
                Device = device
            };

            floatId = "123456";
            floatName = "dummyFloatName";
            floatRank = 1;
            floatRawState = 0.5f;
            floatStateType = "float";
            floatActivatorState = new ActivatorState(floatRawState, floatStateType);
            floatActivator = new Activator(floatId, floatName, floatRank, floatActivatorState)
            {
                Device = device
            };

            Assert.IsNotNull(boolActivator);
            Assert.IsNotNull(floatActivator);
        }

        [TestMethod]
        public void SetAndGetIdTest()
        {
            Assert.AreEqual(boolId, boolActivator.ActivatorId);
            Assert.AreEqual(floatId, floatActivator.ActivatorId);

            string newBoolId = "abc";
            boolActivator.ActivatorId = newBoolId;
            string newFloatId = "123321";
            floatActivator.ActivatorId = newFloatId;

            Assert.AreEqual(newBoolId, boolActivator.ActivatorId);
            Assert.AreEqual(newFloatId, floatActivator.ActivatorId);
        }

        [TestMethod]
        public void SetAndGetNameTest()
        {
            Assert.AreEqual(boolName, boolActivator.Name);
            Assert.AreEqual(floatName, floatActivator.Name);

            string newBoolName = "newBoolName";
            boolActivator.Name = newBoolName;
            string newFloatName = "newFloatName";
            floatActivator.Name = newFloatName;

            Assert.AreEqual(newBoolName, boolActivator.Name);
            Assert.AreEqual(newFloatName, floatActivator.Name);
        }

        [TestMethod]
        public void SetAndGetRankTest()
        {
            Assert.AreEqual(boolRank, boolActivator.Rank);
            Assert.AreEqual(floatRank, floatActivator.Rank);

            int newBoolRank = 1;
            boolActivator.Rank = newBoolRank;
            int newFloatRank = 2;
            floatActivator.Rank = newFloatRank;

            Assert.AreEqual(newBoolRank, boolActivator.Rank);
            Assert.AreEqual(newFloatRank, floatActivator.Rank);
        }

        [TestMethod]
        public void SetAndGetBoolStateTestSuccess()
        {
            Assert.AreEqual(boolActivatorState, boolActivator.State);

            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(dummyAddress, dummyPort);
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            boolActivator.Device.ServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            ActivatorState newBoolState = new ActivatorState(true, "bool");

            Assert.AreNotEqual(boolActivatorState, newBoolState);

            try
            {
                boolActivator.State = newBoolState;
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.AreEqual(newBoolState, boolActivator.State);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void SetBoolStateFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(dummyAddress, dummyPort);
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Throws(new ServerInteractionException());
            boolActivator.Device.ServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            ActivatorState newBoolState = new ActivatorState(true, "bool");
            boolActivator.State = newBoolState;
        }

        [TestMethod]
        public void SetAndGetFloatStateTestSuccess()
        {
            Assert.AreEqual(floatActivatorState, floatActivator.State);

            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(dummyAddress, dummyPort);
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            floatActivator.Device.ServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            ActivatorState newFloatState = new ActivatorState(1.5, "float");

            Assert.AreNotEqual(floatActivatorState, newFloatState);

            try
            {
                floatActivator.State = newFloatState;
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.AreEqual(newFloatState, floatActivator.State);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void SetFloatStateFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(dummyAddress, dummyPort);
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Throws(new ServerInteractionException());
            floatActivator.Device.ServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            ActivatorState newFloatState = new ActivatorState(1.5, "float");
            floatActivator.State = newFloatState;
        }

        [TestMethod]
        public void SetAndGetDeviceTest()
        {
            Assert.AreEqual(device, boolActivator.Device);
            Assert.AreEqual(device, floatActivator.Device);

            Device newDevice = new Device("987", "bob", "...", new List<Activator>(), serverInteractor);

            Assert.IsNotNull(newDevice);
            Assert.AreNotEqual(newDevice, device);

            boolActivator.Device = newDevice;
            floatActivator.Device = newDevice;

            Assert.AreEqual(newDevice, boolActivator.Device);
            Assert.AreEqual(newDevice, floatActivator.Device);
        }
    }
}
