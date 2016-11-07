namespace Ergate.DataAccess.ExceptionHandling
{
    using System;

    internal class ExceptionHandler : IExceptionHandler
	{
		private readonly IExceptionHandler chainHead =
			new SqlExceptionHandler(
				new ConcurrencyExceptionHandler(
					new UpdateExceptionHandler(
						new DbEntityValidationExceptionHandler(
							new DefaultExceptionHandler())))
				);

		public void Handle(Exception exception)
		{
			this.chainHead.Handle(exception);
		}
	}
}