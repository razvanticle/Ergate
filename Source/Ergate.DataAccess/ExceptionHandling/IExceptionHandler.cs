namespace Ergate.DataAccess.ExceptionHandling
{
    using System;

    public interface IExceptionHandler
	{
		void Handle(Exception exception);
	}
}