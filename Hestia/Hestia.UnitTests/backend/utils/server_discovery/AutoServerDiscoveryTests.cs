using Hestia.backend.utils.server_discovery;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hestia.UnitTests.backend.utils.server_discovery
{
    [TestClass]
    public class AutoServerDiscoveryTests
    {
        AutoServerDicovery discoverer;

        [TestInitialize]
        public void Setup()
        {
            discoverer = new AutoServerDicovery();
            Assert.IsNotNull(discoverer);
        }

        [TestMethod]
        public void DiscoverAndStopTest()
        {
            ServerDelegate serverDelegate = discoverer.Delegate;
            discoverer.Search();
            Assert.IsTrue(serverDelegate.Searching);
            discoverer.Stop();
            Assert.IsFalse(serverDelegate.Searching);
        }

        [TestMethod]
        public void GetDelegateTest()
        {
            ServerDelegate serverDelegate = discoverer.Delegate;
            Assert.IsNotNull(serverDelegate);
            Assert.IsFalse(serverDelegate.Searching);
        }
    }
}
