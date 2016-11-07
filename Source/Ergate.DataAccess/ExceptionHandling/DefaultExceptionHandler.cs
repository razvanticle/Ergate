namespace Ergate.DataAccess.ExceptionHandling
{
    using System;

    using Ergate.DataAccess.Exceptions;

    internal class DefaultExceptionHandler : IExceptionHandler
	{
		public void Handle(Exception exception)
		{
			throw new RepositoryViolationException(exception);
		}
	}
}