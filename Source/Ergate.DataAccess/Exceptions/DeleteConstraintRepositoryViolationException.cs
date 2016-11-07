namespace Ergate.DataAccess.Exceptions
{
    using System;
    using System.Data.SqlClient;
    using System.Runtime.Serialization;

    [Serializable]
	public class DeleteConstraintRepositoryViolationException : RepositoryViolationException
	{
		public DeleteConstraintRepositoryViolationException()
		{
		}

		public DeleteConstraintRepositoryViolationException(string errorMessage)
			: base(errorMessage)
		{
		}

		public DeleteConstraintRepositoryViolationException(SqlException exception)
			: base(exception)
		{
		}

		public DeleteConstraintRepositoryViolationException(string message, Exception exception)
			: base(message, exception)
		{
		}

		protected DeleteConstraintRepositoryViolationException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}