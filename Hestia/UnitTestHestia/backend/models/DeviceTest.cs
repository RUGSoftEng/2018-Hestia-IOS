using System;
using Hestia.backend.models;
using NUnit.Framework;

namespace UnitTestHestia.backend.models
{
    [TestFixture]
    public class DeviceTest
    {
        [Test]
        public void ConstructorTest()
        {
            Device device = new Device("testId", "test", "bool", null, null);
            Assert.NotNull(device);
        }

        [Test]
        public void Fail()
        {
            Assert.False(true);
        }

        [Test]
        [Ignore("another time")]
        public void Ignore()
        {
            Assert.True(false);
        }
    }
}