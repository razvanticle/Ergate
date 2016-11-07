namespace Ergate.DataAccess.UnitTests.TestDoubles
{
    using System;

    using Ergate.DataAccess.ExceptionHandling;

    public class FakeExceptionHandler : IExceptionHandler
    {
        public void Handle(Exception exception)
        {
            this.Handled = exception;
        }

        public Exception Handled
        {
            get;
            private set;
        }
    }
}