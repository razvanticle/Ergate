namespace Ergate.DataAccess.UnitTests.ExceptionHandlingTests
{
    using System;

    using Ergate.DataAccess.ExceptionHandling;
    using Ergate.DataAccess.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExceptionHandlerTests
    {
        [TestMethod]
        public void Handle_ExceptionWhichNoSpecificHandlerCanHandle_RepositoryViolationExceptionThrown()
        {
            var handler = new ExceptionHandler();
            var e = new Exception();

            Action act = () => handler.Handle(e);

            act.ShouldThrow<RepositoryViolationException>(actual => ReferenceEquals(actual.InnerException, e));
        }
    }
}