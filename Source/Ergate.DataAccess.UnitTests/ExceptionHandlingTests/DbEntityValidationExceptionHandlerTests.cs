namespace Ergate.DataAccess.UnitTests.ExceptionHandlingTests
{
    using System;
    using System.Data.Entity.Validation;

    using Ergate.DataAccess.ExceptionHandling;
    using Ergate.DataAccess.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class DbEntityValidationExceptionHandlerTests : ExceptionHandlersBaseTests<DataValidationException>
    {
        protected override IExceptionHandler GetTarget(Mock<IExceptionHandler> mock)
        {
            return new DbEntityValidationExceptionHandler(mock.Object);
        }

        protected override Exception GetExpectedInnerException()
        {
            return new DbEntityValidationException();
        }
    }
}