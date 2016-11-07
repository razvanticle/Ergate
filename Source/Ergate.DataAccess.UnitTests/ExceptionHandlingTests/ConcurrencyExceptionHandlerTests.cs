namespace Ergate.DataAccess.UnitTests.ExceptionHandlingTests
{
    using System;
    using System.Data.Entity.Core;

    using Ergate.DataAccess.ExceptionHandling;
    using Ergate.DataAccess.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class ConcurrencyExceptionHandlerTests : ExceptionHandlersBaseTests<ConcurrencyRepositoryViolationException>
    {
        protected override IExceptionHandler GetTarget(Mock<IExceptionHandler> mock)
        {
            return new ConcurrencyExceptionHandler(mock.Object);
        }

        protected override Exception GetExpectedInnerException()
        {
            return new OptimisticConcurrencyException();
        }
    }
}