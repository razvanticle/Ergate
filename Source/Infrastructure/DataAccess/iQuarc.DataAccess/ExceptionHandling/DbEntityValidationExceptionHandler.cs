namespace iQuarc.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Validation;

    using Ergate.Common.Extensions;

    internal class DbEntityValidationExceptionHandler : IExceptionHandler
    {
        private readonly IExceptionHandler successor;

        public DbEntityValidationExceptionHandler(IExceptionHandler successor)
        {
            this.successor = successor;
        }

        public IExceptionHandler Successor { get; private set; }

        public void Handle(Exception exception)
        {
            var validationException = exception.FirstInner<DbEntityValidationException>();
            if (validationException != null)
            {
                var errors = this.GetErrors(validationException);
                throw new DataValidationException(validationException.Message, errors, validationException);
            }

            this.successor.Handle(exception);
        }

        private IEnumerable<ValidationError> GetEntryErrors(IEnumerable<DbValidationError> validationErrors)
        {
            foreach (var dbValidationError in validationErrors)
            {
                yield return new ValidationError(dbValidationError.PropertyName, dbValidationError.ErrorMessage);
            }
        }

        private IEnumerable<DataValidationResult> GetErrors(DbEntityValidationException validationException)
        {
            foreach (var dbEntityValidationResult in validationException.EntityValidationErrors)
            {
                var entry = new EntityEntry(dbEntityValidationResult.Entry);
                var entryErrors = this.GetEntryErrors(dbEntityValidationResult.ValidationErrors);
                yield return new DataValidationResult(entry, entryErrors);
            }
        }
    }
}