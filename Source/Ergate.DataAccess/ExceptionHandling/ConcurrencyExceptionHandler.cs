namespace Ergate.DataAccess.ExceptionHandling
{
    using System;
    using System.Data.Entity.Core;

    using Ergate.Common.Extensions;
    using Ergate.DataAccess.Exceptions;

    internal class ConcurrencyExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandler successor;

        public ConcurrencyExceptionHandler(IExceptionHandler successor)
        {
            this.successor = successor;
        }

        public void Handle(Exception exception)
        {
            var concurrencyException = exception.FirstInner<OptimisticConcurrencyException>();
            if (concurrencyException != null)
            {
                throw new ConcurrencyRepositoryViolationException(concurrencyException);
            }

            this.successor.Handle(exception);
        }
    }
}