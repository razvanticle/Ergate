namespace Ergate.DataAccess.UnitTests.ExceptionHandlingTests
{
    using System;
    using System.Data.Entity.Core;

    using Ergate.DataAccess.ExceptionHandling;
    using Ergate.DataAccess.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class UpdateExceptionHandlerTests : ExceptionHandlersBaseTests<RepositoryUpdateException>
    {
        protected override IExceptionHandler GetTarget(Mock<IExceptionHandler> mock)
        {
            return new UpdateExceptionHandler(mock.Object);
        }

        protected override Exception GetExpectedInnerException()
        {
            return new UpdateException();
        }
    }
}