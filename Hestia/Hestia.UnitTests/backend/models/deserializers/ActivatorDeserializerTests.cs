using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Hestia.UnitTests
{
    [TestClass]
    public class ActivatorDeserializerTest
    {
        [TestMethod]
        public void DeserializeHappyTest()
        {
            Activator testActivator = new Activator("1", "testActivator", 1, new ActivatorState(true, "bool"));

            JObject jObjectTestActivator = JObject.FromObject(testActivator);
            ActivatorDeserializer deserializer = new ActivatorDeserializer();

            Activator newActivator = deserializer.Deserialize(jObjectTestActivator);

            Assert.IsTrue(testActivator.Equals(newActivator));
        }
    }
}
