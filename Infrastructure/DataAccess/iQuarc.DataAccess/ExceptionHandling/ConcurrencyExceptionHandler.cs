using System;
using System.Data.Entity.Core;
using iQuarc.SystemEx;

namespace iQuarc.DataAccess
{
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

			successor.Handle(exception);
		}
	}
}