namespace Ergate.Infrastructure.WebApi
{
    using System;

    public interface IExceptionLogger
	{
		void Log(Exception exception);
	}
}