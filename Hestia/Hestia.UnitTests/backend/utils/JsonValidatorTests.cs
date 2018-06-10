using Hestia.Backend;
using Hestia.Utils.Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Hestia.UnitTests.backend.utils
{
    [TestClass]
    public class JsonValidatorTests
    {
        [TestMethod]
        public void IsValidJsonTestJObjectSuccess()
        {
            JObject json = new JObject
            {
                ["key"] = "value",
                ["anotherKey"] = "anotherValue"
            };
            JObject nestedJson = new JObject
            {
                ["key"] = "value",
                ["anotherKey"] = "anotherValue"
            };
            json["nested"] = nestedJson;

            Assert.IsTrue(JsonValidator.IsValidJson(json.ToString()));
        }

        [TestMethod]
        public void IsValidJsonTestJArraySuccess()
        {
            JArray jsonArray = new JArray();
            JObject arrayEntry1 = new JObject
            {
                ["key1"] = "value1"
            };
            JObject arrayEntry2 = new JObject
            {
                ["key2"] = "value2"
            };
            jsonArray.Add(arrayEntry1);
            jsonArray.Add(arrayEntry2);
            // Add itself cause why not
            jsonArray.Add(jsonArray);

            Assert.IsTrue(JsonValidator.IsValidJson(jsonArray.ToString()));
        }

        [TestMethod]
        public void IsValidJsonTestFailure()
        {
            string notJson = "This is not a json string.";

            Assert.IsFalse(JsonValidator.IsValidJson(notJson));
        }
    }
}
