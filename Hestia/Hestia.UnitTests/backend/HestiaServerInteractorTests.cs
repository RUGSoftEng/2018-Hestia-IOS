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
    public class HestiaServerInteractorTests
    {
        private NetworkHandler dummyNetworkHandler;
        private HestiaServerInteractor dummyServerInteractorLocal;
        private HestiaServerInteractor dummyServerInteractorRemote;
        private string ip = "0.0.0.0";
        private int port = 1000;
        private string dummyServerId = "abc";

        [TestInitialize]
        public void SetUpHestiaServerInteractor()
        {
            dummyNetworkHandler = new NetworkHandler(ip, port);
            dummyServerInteractorLocal = new HestiaServerInteractor(dummyNetworkHandler);
            dummyServerInteractorRemote = new HestiaServerInteractor(dummyNetworkHandler, dummyServerId);

            Assert.IsNotNull(dummyServerInteractorLocal);
            Assert.IsNotNull(dummyServerInteractorRemote);
        }

        [TestMethod]
        public void SetAndGetNetworkHandlerTest()
        {
            Assert.AreEqual(dummyNetworkHandler, dummyServerInteractorLocal.NetworkHandler);

            string newIp = "1.0.0.0";
            int newPort = 2000;
            NetworkHandler newNetworkHandler = new NetworkHandler(newIp, newPort);

            Assert.AreNotEqual(newNetworkHandler, dummyNetworkHandler);

            dummyServerInteractorLocal.NetworkHandler = newNetworkHandler;

            Assert.AreEqual(newNetworkHandler, dummyServerInteractorLocal.NetworkHandler);
        }

        /*
         * GetDevices() tests
         */
        [TestMethod]
        public void GetDevicesTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandlerLocal = new Mock<NetworkHandler>(ip, port);
            Mock<NetworkHandler> mockNetworkHandlerRemote = new Mock<NetworkHandler>(ip, port);

            JArray deviceJsonArray = new JArray();
            JObject deviceJsonObject = new JObject
            {
                ["deviceId"] = "dummyId",
                ["name"] = "dummyName",
                ["type"] = "dummyType",
                ["activators"] = new JArray()
            };
            deviceJsonArray.Add(deviceJsonObject);

            mockNetworkHandlerLocal.CallBase = true;
            mockNetworkHandlerLocal.Setup(x => x.Get(It.IsAny<string>())).Returns(deviceJsonArray);
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandlerLocal.Object;

            mockNetworkHandlerRemote.CallBase = true;
            mockNetworkHandlerRemote.Setup(x => x.Post(It.IsAny<JObject>() ,It.IsAny<string>())).Returns(deviceJsonArray);
            dummyServerInteractorRemote.NetworkHandler = mockNetworkHandlerRemote.Object;

            List<Device> devicesLocal = null;
            try
            {
                devicesLocal = dummyServerInteractorLocal.GetDevices();
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            List<Device> devicesRemote = null;
            try
            {
                devicesRemote = dummyServerInteractorRemote.GetDevices();
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(devicesLocal);
            Assert.AreEqual(devicesLocal[0].DeviceId, "dummyId");
            Assert.AreEqual(devicesLocal[0].Name, "dummyName");
            Assert.AreEqual(devicesLocal[0].Type, "dummyType");
            Assert.IsNotNull(devicesLocal[0].Activators);
            Assert.AreEqual(devicesLocal[0].ServerInteractor.NetworkHandler.Ip, dummyNetworkHandler.Ip);
            Assert.AreEqual(devicesLocal[0].ServerInteractor.NetworkHandler.Port, dummyNetworkHandler.Port);

            Assert.IsNotNull(devicesRemote);
            Assert.AreEqual(devicesRemote[0].DeviceId, "dummyId");
            Assert.AreEqual(devicesRemote[0].Name, "dummyName");
            Assert.AreEqual(devicesRemote[0].Type, "dummyType");
            Assert.IsNotNull(devicesRemote[0].Activators);
            Assert.AreEqual(devicesRemote[0].ServerInteractor.NetworkHandler.Ip, dummyNetworkHandler.Ip);
            Assert.AreEqual(devicesRemote[0].ServerInteractor.NetworkHandler.Port, dummyNetworkHandler.Port);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetDevicesTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;

            List<Device> devices = dummyServerInteractorLocal.GetDevices();
        }

        /*
         * AddDevice() tests
         */
        [TestMethod]
        public void AddDeviceTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;
            dummyServerInteractorRemote.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            Dictionary<string, string> requiredInfo = new Dictionary<string, string>();
            PluginInfo pluginInfo = new PluginInfo(collection, plugin, requiredInfo);

            try
            {
                dummyServerInteractorLocal.AddDevice(pluginInfo);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            try
            {
                dummyServerInteractorRemote.AddDevice(pluginInfo);
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void AddDeviceTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            Dictionary<string, string> requiredInfo = new Dictionary<string, string>();
            PluginInfo pluginInfo = new PluginInfo(collection, plugin, requiredInfo);

            dummyServerInteractorLocal.AddDevice(pluginInfo);
        }

        /*
         * RemoveDevice() tests
         */
        [TestMethod]
        public void RemoveDeviceTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandlerLocal = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandlerLocal.CallBase = true;
            mockNetworkHandlerLocal.Setup(x => x.Delete(It.IsAny<string>())).Returns(new JObject());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandlerLocal.Object;

            Mock<NetworkHandler> mockNetworkHandlerRemote = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandlerRemote.CallBase = true;
            mockNetworkHandlerRemote.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JObject());
            dummyServerInteractorRemote.NetworkHandler = mockNetworkHandlerRemote.Object;

            string deviceId = "dummyId";
            string name = "dummyName";
            string type = "dummyType";
            List<Activator> activators = new List<Activator>();
            Device device = new Device(deviceId, name, type, activators, dummyServerInteractorLocal);

            try
            {
                dummyServerInteractorLocal.RemoveDevice(device);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            try
            {
                dummyServerInteractorRemote.RemoveDevice(device);
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void RemoveDeviceTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Delete(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;

            string deviceId = "dummyId";
            string name = "dummyName";
            string type = "dummyType";
            List<Activator> activators = new List<Activator>();
            Device device = new Device(deviceId, name, type, activators, dummyServerInteractorLocal);

            dummyServerInteractorLocal.RemoveDevice(device);
         }

        /*
         * GetCollections() tests 
         */
        [TestMethod]
        public void GetCollectionsTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandlerLocal = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandlerLocal.CallBase = true;
            mockNetworkHandlerLocal.Setup(x => x.Get(It.IsAny<string>())).Returns(new JArray());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandlerLocal.Object;

            Mock<NetworkHandler> mockNetworkHandlerRemote = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandlerRemote.CallBase = true;
            mockNetworkHandlerRemote.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JArray());
            dummyServerInteractorRemote.NetworkHandler = mockNetworkHandlerRemote.Object;

            List<string> collectionLocal = null;
            try
            {
                collectionLocal = dummyServerInteractorLocal.GetCollections();
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            List<string> collectionRemote = null;
            try
            {
                collectionRemote = dummyServerInteractorRemote.GetCollections();
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(collectionLocal);
            Assert.IsNotNull(collectionRemote);
            Assert.IsTrue(collectionLocal.Count == 0);
            Assert.IsTrue(collectionRemote.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetCollectionsTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;

            List<string> collections = dummyServerInteractorLocal.GetCollections();
        }

        /*
         * GetPlugins() tests
         */
        [TestMethod]
        public void GetPluginsTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandlerLocal = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandlerLocal.CallBase = true;
            mockNetworkHandlerLocal.Setup(x => x.Get(It.IsAny<string>())).Returns(new JArray());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandlerLocal.Object;

            Mock<NetworkHandler> mockNetworkHandlerRemote = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandlerRemote.CallBase = true;
            mockNetworkHandlerRemote.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(new JArray());
            dummyServerInteractorRemote.NetworkHandler = mockNetworkHandlerRemote.Object;

            string collection = "dummyCollection";
            List<string> pluginsLocal = null;
            try
            {
                pluginsLocal = dummyServerInteractorLocal.GetPlugins(collection);
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            List<string> pluginsRemote = null;
            try
            {
                pluginsRemote = dummyServerInteractorRemote.GetPlugins(collection);
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(pluginsLocal);
            Assert.IsNotNull(pluginsRemote);
            Assert.IsTrue(pluginsLocal.Count == 0);
            Assert.IsTrue(pluginsRemote.Count == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetPluginsTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            List<string> plugins = dummyServerInteractorLocal.GetPlugins(collection);
        }

        /*
         * GetPluginInfo() and GetRequiredPluginInfo() tests
         */
        [TestMethod]
        public void GetPluginInfoAndGetRequiredPluginInfoTestSuccess()
        {
            Mock<NetworkHandler> mockNetworkHandlerLocal = new Mock<NetworkHandler>(ip, port);
            Mock<NetworkHandler> mockNetworkHandlerRemote = new Mock<NetworkHandler>(ip, port);

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

            mockNetworkHandlerLocal.CallBase = true;
            mockNetworkHandlerLocal.Setup(x => x.Get(It.IsAny<string>())).Returns(pluginInfoJson);
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandlerLocal.Object;

            mockNetworkHandlerRemote.CallBase = true;
            mockNetworkHandlerRemote.Setup(x => x.Post(It.IsAny<JObject>(), It.IsAny<string>())).Returns(pluginInfoJson);
            dummyServerInteractorRemote.NetworkHandler = mockNetworkHandlerRemote.Object;

            PluginInfo pluginInfoLocal = null;
            try
            {
                pluginInfoLocal = dummyServerInteractorLocal.GetPluginInfo(collection, plugin);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            PluginInfo pluginInfoRemote = null;
            try
            {
                pluginInfoRemote = dummyServerInteractorRemote.GetPluginInfo(collection, plugin);
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(pluginInfoLocal);
            Assert.IsNotNull(pluginInfoRemote);
            Assert.AreEqual(collection, pluginInfoLocal.Collection);
            Assert.AreEqual(collection, pluginInfoRemote.Collection);
            Assert.AreEqual(plugin, pluginInfoLocal.Plugin);
            Assert.AreEqual(plugin, pluginInfoRemote.Plugin);
            CollectionAssert.AreEqual(requiredInfo, pluginInfoLocal.RequiredInfo);
            CollectionAssert.AreEqual(requiredInfo, pluginInfoRemote.RequiredInfo);

            // Test for GetRequiredPluginInfo()
            Dictionary<string, string> requiredPluginInfoLocal = null;
            try
            {
                requiredPluginInfoLocal = dummyServerInteractorLocal.GetRequiredPluginInfo(collection, plugin);
            } catch(ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Dictionary<string, string> requiredPluginInfoRemote = null;
            try
            {
                requiredPluginInfoRemote = dummyServerInteractorRemote.GetRequiredPluginInfo(collection, plugin);
            }
            catch (ServerInteractionException ex)
            {
                Assert.Fail(ex.Message, ex);
            }

            Assert.IsNotNull(requiredPluginInfoLocal);
            Assert.IsNotNull(requiredPluginInfoRemote);
            CollectionAssert.AreEqual(requiredInfo, requiredPluginInfoLocal);
            CollectionAssert.AreEqual(requiredInfo, requiredPluginInfoRemote);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetPluginInfoTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            PluginInfo pluginInfo = dummyServerInteractorLocal.GetPluginInfo(collection, plugin);
        }

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void GetRequiredPluginInfoTestFailure()
        {
            Mock<NetworkHandler> mockNetworkHandler = new Mock<NetworkHandler>(ip, port);
            mockNetworkHandler.CallBase = true;
            mockNetworkHandler.Setup(x => x.Get(It.IsAny<string>())).Throws(new ServerInteractionException());
            dummyServerInteractorLocal.NetworkHandler = mockNetworkHandler.Object;

            string collection = "dummyCollection";
            string plugin = "dummyPlugin";
            Dictionary<string, string> requiredInfo = dummyServerInteractorLocal.GetRequiredPluginInfo(collection, plugin);
        }
    }
}
