using Hestia.Backend.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hestia.UnitTests.Backend.Models
{
    [TestClass]
    public class ActivatorStateTests
    {
        private bool boolRawState;
        private string boolType;
        private ActivatorState boolState;

        private float floatRawState;
        private string floatType;
        private ActivatorState floatState;

        [TestInitialize]
        public void SetUpActivatorStates()
        {
            boolRawState = true;
            boolType = "bool";
            boolState = new ActivatorState(boolRawState, boolType);

            floatRawState = 0.5f;
            floatType = "float";
            floatState = new ActivatorState(floatRawState, floatType);

            Assert.IsNotNull(boolState);
            Assert.IsNotNull(floatState);
        }

        [TestMethod]
        public void SetAndGetBoolRawStateTest()
        {
            Assert.AreEqual(boolRawState, boolState.RawState);
            bool newBoolRawState = false;
            boolState.RawState = newBoolRawState;
            Assert.AreEqual(newBoolRawState, boolState.RawState);
        }

        [TestMethod]
        public void SetAndGetFloatRawStateTest()
        {
            Assert.AreEqual(floatRawState, floatState.RawState);
            float newFloatRawState = 0.75f;
            floatState.RawState = newFloatRawState;
            Assert.AreEqual(newFloatRawState, floatState.RawState);
        }

        [TestMethod]
        public void SetAndGetBoolTypeTest()
        {
            Assert.AreEqual(boolType, boolState.Type);
            string newBoolType = "newBoolType";
            boolState.Type = newBoolType;
            Assert.AreEqual(newBoolType, boolState.Type);
        }

        [TestMethod]
        public void SetAndGetFloatTypeTest()
        {
            Assert.AreEqual(floatType, floatState.Type);
            string newFloatType = "newFloatType";
            floatState.Type = newFloatType;
            Assert.AreEqual(newFloatType, floatState.Type);
        }
    }
}
