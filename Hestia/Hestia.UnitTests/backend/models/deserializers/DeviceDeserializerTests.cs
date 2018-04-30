using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Hestia.UnitTests.backend
{
    [TestClass]
    public class DeviceDeserializerTests
    {
        [TestMethod]
        public void DeserializeHappyTest()
        {
            Device testDevice = new Device("1", "deviceName", "deviceType", new List<Activator>(), new Hestia.backend.NetworkHandler("ip", 8000));

            DeviceDeserializer deserializer = new DeviceDeserializer();
            JObject jObjectTestDevice = JObject.FromObject(testDevice);

            Device newDevice = deserializer.Deserialize(jObjectTestDevice, new Hestia.backend.NetworkHandler("ip", 8000));

            Assert.IsTrue(testDevice.Equals(newDevice));
        }
    }
}
