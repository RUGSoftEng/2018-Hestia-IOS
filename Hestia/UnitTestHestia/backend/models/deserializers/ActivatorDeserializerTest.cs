using Hestia.backend.models;
using NUnit.Framework;
using Newtonsoft.Json.Linq;
using Hestia.backend.models.deserializers;

namespace UnitTestHestia.backend.models.deserializers
{
    [TestFixture]
    class ActivatorDeserializerTest
    {
        [Test]
        public void DeserializeHappyTest()
        {
            Activator testActivator = new Activator("1", "testActivator", 1, new ActivatorState<object>(true, "bool"));

            JObject jObjectTestActivator = JObject.FromObject(testActivator);
            ActivatorDeserializer deserializer = new ActivatorDeserializer();

            Activator newActivator = deserializer.Deserialize(jObjectTestActivator);

            Assert.True(testActivator.Equals(newActivator));
        }
    }
}