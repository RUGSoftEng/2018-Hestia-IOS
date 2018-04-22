using Hestia.backend.models;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Hestia.backend.models.deserializers;
using System.Collections.Generic;

namespace UnitTestHestia.backend.models.deserializers
{
    [TestFixture]
    class DeviceDeserializerTest
    {
        [Test]
        public void DeserializeHappyTest()
        {
            Device testDevice = new Device("1", "deviceName", "deviceType", new List<Activator>(), new Hestia.backend.NetworkHandler("ip", 8000));

            DeviceDeserializer deserializer = new DeviceDeserializer();
            JObject jObjectTestDevice = JObject.FromObject(testDevice);

            Device newDevice = deserializer.Deserialize(jObjectTestDevice, new Hestia.backend.NetworkHandler("ip", 8000));

            Assert.True(testDevice.Equals(newDevice));
        }
    }
}
