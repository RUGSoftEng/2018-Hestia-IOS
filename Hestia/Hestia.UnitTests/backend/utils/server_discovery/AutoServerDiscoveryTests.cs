using System;

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
        public void DelegateTest()
        {
            Assert.IsNotNull(discoverer.Delegate);  
        }

        [TestMethod]
        public void DiscoverTest()
        {

        }
    }
}
