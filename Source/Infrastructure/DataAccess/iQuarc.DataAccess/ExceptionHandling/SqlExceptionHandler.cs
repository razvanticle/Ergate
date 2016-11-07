namespace iQuarc.DataAccess
{
    using System;
    using System.Data.SqlClient;

    using Ergate.Common.Extensions;

    internal class SqlExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandler successor;

        public SqlExceptionHandler(IExceptionHandler successor)
        {
            this.successor = successor;
        }

        public void Handle(Exception exception)
        {
            var sqlException = exception.FirstInner<SqlException>();
            if (sqlException != null)
            {
                switch (sqlException.Number)
                {
                    case 242:
                        throw new DateTimeRangeRepositoryViolationException(sqlException);
                    case 547:
                        throw new DeleteConstraintRepositoryViolationException(sqlException);
                    case 1205:
                        throw new DeadlockVictimRepositoryViolationException(sqlException);
                    case 2601:
                    case 2627:
                        throw new UniqueConstraintRepositoryViolationException(sqlException);
                    default:
                        throw new RepositoryViolationException(sqlException);
                }
            }

            this.successor.Handle(exception);
        }
    }
}