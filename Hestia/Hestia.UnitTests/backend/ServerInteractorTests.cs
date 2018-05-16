using Hestia.backend;
using Hestia.backend.exceptions;
using Hestia.backend.models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.UnitTests.backend
{
    [TestClass]
    public class ServerInteractorTests
    {
        private NetworkHandler dummyNetworkHandler;
        private HestiaServerInteractor dummyServerInteractor;
        private string ip = "0.0.0.0";
        private int port = 1000;

        [TestInitialize]
        public void SetUpServerInteractor()
        {
            dummyNetworkHandler = new NetworkHandler(ip, port);
            dummyServerInteractor = new HestiaServerInteractor(dummyNetworkHandler);

            Assert.IsNotNull(dummyServerInteractor);
        }

        [TestMethod]
        public void SetAndGetNetworkHandlerTest()
        {
            Assert.AreEqual(dummyNetworkHandler, dummyServerInteractor.NetworkHandler);

            string newIp = "1.0.0.0";
            int newPort = 2000;
            NetworkHandler newNetworkHandler = new NetworkHandler(newIp, newPort);

            Assert.AreNotEqual(newNetworkHandler, dummyNetworkHandler);

            dummyServerInteractor.NetworkHandler = newNetworkHandler;

            Assert.AreEqual(newNetworkHandler, dummyServerInteractor.NetworkHandler);
        }

        /*
         * GetDevices() tests
         */
        [TestMethod]
        public void GetDevicesTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);

            JArray deviceJsonArray = new JArray();
            JObject deviceJsonObject = new JObject
            {
                ["deviceId"] = "dummyId",
                ["name"] = "dummyName",
                ["type"] = "dummyType",
                ["activators"] = new JArray()
            };
            deviceJsonArray.Add(deviceJsonObject);

            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Returns(deviceJsonArray);
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            List<Device> devices = null;
            try
            {
                devices = dummyServerInteractor.GetDevices();
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(devices);
            Assert.AreEqual(devices[0].DeviceId, "dummyId");
            Assert.AreEqual(devices[0].Name, "dummyName");
            Assert.AreEqual(devices[0].Type, "dummyType");
            Assert.IsNotNull(devices[0].Activators);
            Assert.AreEqual(devices[0].ServerInteractor.NetworkHandler.Ip, dummyNetworkHandler.Ip);
            Assert.AreEqual(devices[0].ServerInteractor.NetworkHandler.Port, dummyNetworkHandler.Port);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetDevicesTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            List<Device> devices = dummyServerInteractor.GetDevices();
        }

        /*
         * AddDevice() tests
         */
        [TestMethod]
        public void AddDeviceTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            Dictionary<string, string> requiredInfo = new Dictionary<string, string>();
            PluginInfo pluginInfo = new PluginInfo(collection, plugin, requiredInfo);

            try
            {
                dummyServerInteractor.AddDevice(pluginInfo);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void AddDeviceTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            Dictionary<string, string> requiredInfo = new Dictionary<string, string>();
            PluginInfo pluginInfo = new PluginInfo(collection, plugin, requiredInfo);

            dummyServerInteractor.AddDevice(pluginInfo);
        }

        /*
         * RemoveDevice() tests
         */
        [TestMethod]
        public void RemoveDeviceTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Delete(It.IsAny<string>())).Returns(new JObject());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string deviceId = "dummyId";
            string name = "dummyName";
            string type = "dummyType";
            List<Activator> activators = new List<Activator>();
            Device device = new Device(deviceId, name, type, activators, dummyServerInteractor);

            try
            {
                dummyServerInteractor.RemoveDevice(device);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void RemoveDeviceTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Delete(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string deviceId = "dummyId";
            string name = "dummyName";
            string type = "dummyType";
            List<Activator> activators = new List<Activator>();
            Device device = new Device(deviceId, name, type, activators, dummyServerInteractor);

            dummyServerInteractor.RemoveDevice(device);
         }

        /*
         * GetCollections() tests 
         */
        [TestMethod]
        public void GetCollectionsTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Returns(new JArray());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            List<string> collections = null;
            try
            {
                collections = dummyServerInteractor.GetCollections();
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(collections);
            Assert.IsTrue(collections.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetCollectionsTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            List<string> collections = dummyServerInteractor.GetCollections();
        }

        /*
         * GetPlugins() tests
         */
        [TestMethod]
        public void GetPluginsTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Returns(new JArray());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            List<string> plugins = null;
            try
            {
                plugins = dummyServerInteractor.GetPlugins(collection);
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(plugins);
            Assert.IsTrue(plugins.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetPluginsTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            List<string> plugins = dummyServerInteractor.GetPlugins(collection);
        }

        /*
         * GetPluginInfo() and GetRequiredPluginInfo() tests
         */
        [TestMethod]
        public void GetPluginInfoAndGetRequiredPluginInfoTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            string requiredInfoKey = "dummyKey";
            string requiredInfoValue = "dummyValue";
            Dictionary<string, string> requiredInfo = new Dictionary<string, string>
            {
                { requiredInfoKey, requiredInfoValue }
            };

            JObject pluginInfoJson = new JObject
            {
                ["collection"] = collection,
                ["plugin_name"] = plugin,
            };
            JObject requiredInfoJson = new JObject
            {
                [requiredInfoKey] = requiredInfoValue
            };
            pluginInfoJson["required_info"] = requiredInfoJson;

            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Returns(pluginInfoJson);
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            PluginInfo pluginInfo = null;
            try
            {
                pluginInfo = dummyServerInteractor.GetPluginInfo(collection, plugin);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(pluginInfo);
            Assert.AreEqual(collection, pluginInfo.Collection);
            Assert.AreEqual(plugin, pluginInfo.Plugin);
            CollectionAssert.AreEqual(requiredInfo, pluginInfo.RequiredInfo);

            // Test for GetRequiredPluginInfo()
            Dictionary<string, string> requiredPluginInfo = null;
            try
            {
                requiredPluginInfo = dummyServerInteractor.GetRequiredPluginInfo(collection, plugin);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(requiredPluginInfo);
            CollectionAssert.AreEqual(requiredInfo, requiredPluginInfo);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetPluginInfoTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            PluginInfo pluginInfo = dummyServerInteractor.GetPluginInfo(collection, plugin);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetRequiredPluginInfoTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractor.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            Dictionary<string, string> requiredInfo = dummyServerInteractor.GetRequiredPluginInfo(collection, plugin);
        }
    }
}
