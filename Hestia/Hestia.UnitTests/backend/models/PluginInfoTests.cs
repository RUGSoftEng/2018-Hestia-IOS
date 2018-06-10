using Hestia.Backend.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace Hestia.UnitTests.backend.models
{
    [TestClass]
    public class PluginInfoTests
    {
        private string collection;
        private string plugin;
        private Dictionary<string, string> requiredInfo;
        private PluginInfo pluginInfo;

        [TestInitialize]
        public void SetUpPluginInfo()
        {
            collection = "dummyCollection";
            plugin = "dummyPlugin";
            requiredInfo = new Dictionary<string, string>
            {
                {"dummyKey", "dummyValue"}
            };

            pluginInfo = new PluginInfo(collection, plugin, requiredInfo);

            Assert.IsNotNull(pluginInfo);
        }

        [TestMethod]
        public void SetAndGetCollectionTest()
        {
            Assert.AreEqual(collection, pluginInfo.Collection);
            string newCollection = "newCollection";
            pluginInfo.Collection = newCollection;
            Assert.AreEqual(newCollection, pluginInfo.Collection);
        }

        [TestMethod]
        public void SetAndGetPluginTest()
        {
            Assert.AreEqual(plugin, pluginInfo.Plugin);
            string newPlugin = "newPlugin";
            pluginInfo.Plugin = newPlugin;
            Assert.AreEqual(newPlugin, pluginInfo.Plugin);
        }

        [TestMethod]
        public void SetAndGetRequiredInfoTest()
        {
            Assert.AreEqual(requiredInfo, pluginInfo.RequiredInfo);
            Dictionary<string, string> newRequiredInfo = new Dictionary<string, string>
            {
                {"newDummyKey", "newDummyValue"}
            };
            pluginInfo.RequiredInfo = newRequiredInfo;
            CollectionAssert.AreEqual(newRequiredInfo, pluginInfo.RequiredInfo);
        }
    }
}
