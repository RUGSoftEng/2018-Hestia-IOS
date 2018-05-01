using Hestia.backend.exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Hestia.UnitTests.backend.exceptions
{
    [TestClass]
    public class ServerInteractionExceptionTests
    {
        private string message = "something went wrong";

        [TestMethod]
        [ExpectedException(typeof(ServerInteractionException))]
        public void ThrowTest()
        {
            throw new ServerInteractionException();
        }

        [TestMethod]
        public void SetMessageTest()
        {
            try
            {
                throw new ServerInteractionException(message);
            } catch(ServerInteractionException ex)
            {
                Assert.AreEqual(message, ex.Message);
            }
        }

        [TestMethod]
        public void InnerExceptionTest()
        {
            Exception inner = new Exception();

            try
            {
                throw new ServerInteractionException(message, inner);
            }
            catch (ServerInteractionException ex)
            {
                Assert.AreEqual(inner, ex.InnerException);
            }
        }
    }
}
