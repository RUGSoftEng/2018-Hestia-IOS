using Hestia.backend;
using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.UnitTests.backend.models.deserializers
{
    [TestClass]
    public class DeviceDeserializerTests
    {
        HestiaServerInteractor serverInteractor;
        string jsonDeviceString =
              "{\"activators\": ["
            + "{\"rank\": 0,"
            + "\"state\": false,"
            + "\"type\": \"bool\","
            + "\"activatorId\": \"5ad8406be82b3f062ba83086\","
            + "\"name\": \"On/Off\"}],"
            + "\"deviceId\": \"5ad8406be82b3f062ba83088\","
            + "\"type\": \"Light\","
            + "\"name\": \"Bobs lamp\"}";
        string jsonDevicesString =
              "[{\"activators\": ["
            + "{\"rank\": 0,"
            + "\"state\": false,"
            + "\"type\": \"bool\","
            + "\"activatorId\": \"5ad8406be82b3f062ba83086\","
            + "\"name\": \"On/Off\"},"
            + "{\"rank\": 1,"
            + "\"state\": 0.3233333,"
            + "\"type\": \"float\","
            + "\"activatorId\": \"5ad8406be82b3f062ba83087\","
            + "\"name\": \"Dimmer\"}],"
            + "\"deviceId\": \"5ad8406be82b3f062ba83088\","
            + "\"type\": \"Light\","
            + "\"name\": \"Joops lamp\"},"
            + "{\"activators\": ["
            + "{\"rank\": 1,"
            + "\"state\": 0.183333278,"
            + "\"type\": \"float\","
            + "\"activatorId\": \"5adde1f4e82b3f062ba8308c\","
            + "\"name\": \"Dimmer\"},"
            + "{\"rank\": 0,"
            + "\"state\": false,"
            + "\"type\": \"bool\","
            + "\"activatorId\": \"5adde1f4e82b3f062ba8308b\","
            + "\"name\": \"On/Off\"}],"
            + "\"deviceId\": \"5adde1f4e82b3f062ba8308d\","
            + "\"type\": \"Light\","
            + "\"name\": \"Light 6\"}]";
        JToken jsonDevice;
        JToken jsonDevices;

        [TestInitialize]
        public void SetUp()
        {
            string dummyIp = "0.0.0.0";
            int dummyPort = 1000;
            NetworkHandler networkHandler = new NetworkHandler(dummyIp, dummyPort);
            serverInteractor = new HestiaServerInteractor(networkHandler);

            jsonDevice = JToken.Parse(jsonDeviceString);
            jsonDevices = JToken.Parse(jsonDevicesString);
        }

        [TestMethod]
        public void DeserializeDeviceTest()
        {
            DeviceDeserializer deserializer = new DeviceDeserializer();
            Device device = deserializer.DeserializeDevice(jsonDevice, serverInteractor);

            Assert.IsNotNull(device);
            Assert.AreEqual(jsonDevice.Value<string>("deviceId"), device.DeviceId);
            Assert.AreEqual(jsonDevice.Value<string>("name"), device.Name);
            Assert.AreEqual(jsonDevice.Value<string>("type"), device.Type);
            Assert.IsNotNull(device.Activators);
            JToken activators = jsonDevice.SelectToken("activators");
            Assert.AreEqual(activators[0].Value<string>("name"), device.Activators[0].Name);
            Assert.AreEqual(serverInteractor, device.ServerInteractor);
        }

        [TestMethod]
        public void DeserializeDevicesTest()
        {
            DeviceDeserializer deserializer = new DeviceDeserializer();
            List<Device> devices = deserializer.DeserializeDevices(jsonDevices, serverInteractor);

            Assert.IsNotNull(devices);

            Device device1 = devices[0];
            Device device2 = devices[1];

            Assert.IsNotNull(device1);
            JToken jsonDevice1 = jsonDevices[0];
            Assert.AreEqual(jsonDevice1.Value<string>("deviceId"), device1.DeviceId);
            Assert.AreEqual(jsonDevice1.Value<string>("name"), device1.Name);
            Assert.AreEqual(jsonDevice1.Value<string>("type"), device1.Type);
            Assert.IsNotNull(device1.Activators);
            JToken activators1 = jsonDevice1.SelectToken("activators");
            Assert.AreEqual(activators1[0].Value<string>("name"), device1.Activators[0].Name);
            Assert.AreEqual(serverInteractor, device1.ServerInteractor);

            Assert.IsNotNull(device2);
            JToken jsonDevice2 = jsonDevices[1];
            Assert.AreEqual(jsonDevice2.Value<string>("deviceId"), device2.DeviceId);
            Assert.AreEqual(jsonDevice2.Value<string>("name"), device2.Name);
            Assert.AreEqual(jsonDevice2.Value<string>("type"), device2.Type);
            Assert.IsNotNull(device2.Activators);
            JToken activators2 = jsonDevice2.SelectToken("activators");
            Assert.AreEqual(activators2[0].Value<string>("name"), device2.Activators[0].Name);
            Assert.AreEqual(serverInteractor, device2.ServerInteractor);
        }
    }
}
