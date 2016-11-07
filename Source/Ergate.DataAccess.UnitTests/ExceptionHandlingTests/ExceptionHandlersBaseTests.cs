namespace Ergate.DataAccess.UnitTests.ExceptionHandlingTests
{
    using System;

    using Ergate.DataAccess.ExceptionHandling;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    public abstract class ExceptionHandlersBaseTests<TException>
        where TException : Exception
    {
        [TestMethod]
        public void Handle_InnerIsConcurrencyException_ExceptionWrappedAndThrown()
        {
            Exception innerException = this.GetExpectedInnerException();
            Exception ex = new Exception(string.Empty, innerException);

            IExceptionHandler target = this.GetTarget();

            Action act = () => target.Handle(ex);

            act.ShouldThrow<TException>(
                e => ReferenceEquals(e.InnerException, innerException));
        }


        [TestMethod]
        public void Handle_InnerIsNotExpectedException_SuccessorHandles()
        {
            Mock<IExceptionHandler> mock = new Mock<IExceptionHandler>();
            IExceptionHandler target = this.GetTarget(mock);
            Exception ex = new Exception(string.Empty, new Exception());

            target.Handle(ex);

            mock.Verify(h => h.Handle(ex), Times.Once);
        }

        private IExceptionHandler GetTarget()
        {
            return this.GetTarget(new Mock<IExceptionHandler>());
        }

        protected abstract IExceptionHandler GetTarget(Mock<IExceptionHandler> mock);
        protected abstract Exception GetExpectedInnerException();
    }
}