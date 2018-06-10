using Hestia.Backend.Utils.ServerDiscovery;
using Hestia.Frontend.Local;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hestia.UnitTests.Backend.Utils.ServerDiscovery
{
    [TestClass]
    public class AutoServerDiscoveryTests
    {
        AutoServerDicovery discoverer;

        [TestInitialize]
        public void Setup()
        {
            discoverer = new AutoServerDicovery(new UITableViewControllerServerDiscovery(new System.IntPtr()));
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
