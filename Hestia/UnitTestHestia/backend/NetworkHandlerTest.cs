using Hestia.backend;
using Hestia.Resources;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace UnitTestHestia.backend
{
    [TestFixture]
    public class NetworkHandlerTest
    {
        private NetworkHandler dummyNetworkHandler;
        private string dummyIp = "0.0.0.0";
        private int dummyPort = 1000;

        [SetUp]
        public void SetUpNetworkHandler()
        {
            dummyNetworkHandler = new NetworkHandler(dummyIp, dummyPort);
            Assert.IsNotNull(dummyNetworkHandler);
        }

        [Test]
        public void SetAndGetIpTest()
        {
            Assert.AreEqual(dummyIp, dummyNetworkHandler.Ip);
            string newIp = "94.212.164.28";
            dummyNetworkHandler.Ip = newIp;
            Assert.AreEqual(newIp, dummyNetworkHandler.Ip);
        }

        [Test]
        public void SetAndGetPortTest()
        {
            Assert.AreEqual(dummyPort, dummyNetworkHandler.Port);
            int newPort = 8000;
            dummyNetworkHandler.Port = newPort;
            Assert.AreEqual(newPort, dummyNetworkHandler.Port);
        }

        [Test]
        public void ExecuteGetRequestTest()
        {
            string endpoint = strings.devicePath;
            JToken response = dummyNetworkHandler.Get(endpoint);
            Assert.IsTrue(JsonValidator.IsValidJson(response.ToString()));
        }
    }
}