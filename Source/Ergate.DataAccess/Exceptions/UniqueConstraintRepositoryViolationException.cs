namespace Ergate.DataAccess.Exceptions
{
    using System;
    using System.Data.SqlClient;
    using System.Runtime.Serialization;

    [Serializable]
	public class UniqueConstraintRepositoryViolationException : RepositoryViolationException
	{
		public UniqueConstraintRepositoryViolationException()
		{
		}

		public UniqueConstraintRepositoryViolationException(string errorMessage)
			: base(errorMessage)
		{
		}

		public UniqueConstraintRepositoryViolationException(SqlException exception)
			: base(exception)
		{
		}

		public UniqueConstraintRepositoryViolationException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected UniqueConstraintRepositoryViolationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}