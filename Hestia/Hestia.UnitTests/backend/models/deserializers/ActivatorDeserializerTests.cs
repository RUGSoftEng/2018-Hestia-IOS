using Hestia.backend.models;
using Hestia.backend.models.deserializers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Hestia.UnitTests.backend.models.deserializers
{
    [TestClass]
    public class ActivatorDeserializerTest
    {
        private string jsonBoolActivatorString =
            "{\"state\": false,\"type\": \"bool\",\"name\": \"On/Off\",\"activatorId\": \"dummyId\",\"rank\": 0}";
        private string jsonFloatActivatorString =
            "{\"state\": 0.5,\"type\": \"float\",\"name\": \"Slider\",\"activatorId\": \"dummyId\",\"rank\": 1}";
        private JToken jsonBoolActivator, jsonFloatActivator;

        [TestInitialize]
        public void SetUpJsonActivators()
        {
            jsonBoolActivator = JToken.Parse(jsonBoolActivatorString);
            jsonFloatActivator = JToken.Parse(jsonFloatActivatorString);
        }

        [TestMethod]
        public void DeserializeBoolActivatorTest()
        {
            ActivatorDeserializer deserializer = new ActivatorDeserializer();
            Activator boolActivator = deserializer.DeserializeActivator(jsonBoolActivator);

            Assert.IsNotNull(boolActivator);
            Assert.AreEqual(jsonBoolActivator.Value<bool>("state"), boolActivator.State.RawState);
            Assert.AreEqual(jsonBoolActivator.Value<string>("type"), boolActivator.State.Type);
            Assert.AreEqual(jsonBoolActivator.Value<string>("name"), boolActivator.Name);
            Assert.AreEqual(jsonBoolActivator.Value<string>("activatorId"), boolActivator.ActivatorId);
            Assert.AreEqual(jsonBoolActivator.Value<int>("rank"), boolActivator.Rank);
        }

        [TestMethod]
        public void DeserializeFloatActivatorTest()
        {
            ActivatorDeserializer deserializer = new ActivatorDeserializer();
            Activator floatActivator = deserializer.DeserializeActivator(jsonFloatActivator);

            Assert.IsNotNull(floatActivator);
            Assert.AreEqual(jsonFloatActivator.Value<float>("state"), floatActivator.State.RawState);
            Assert.AreEqual(jsonFloatActivator.Value<string>("type"), floatActivator.State.Type);
            Assert.AreEqual(jsonFloatActivator.Value<string>("name"), floatActivator.Name);
            Assert.AreEqual(jsonFloatActivator.Value<string>("activatorId"), floatActivator.ActivatorId);
            Assert.AreEqual(jsonFloatActivator.Value<int>("rank"), floatActivator.Rank);
        }
    }
}
