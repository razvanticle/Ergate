namespace Ergate.Infrastructure.WebApi
{
    using System;
    using System.Diagnostics;

    internal class DebugExceptionLogger : IExceptionLogger
	{
		public void Log(Exception exception)
		{
			Debug.WriteLine(exception);
		}
	}
}