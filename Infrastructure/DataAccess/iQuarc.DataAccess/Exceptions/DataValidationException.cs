﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace iQuarc.DataAccess
{
    [Serializable]
    public class DataValidationException : RepositoryViolationException
    {
        private const string ErrorsKey = "ErrorsKey";
        private readonly DataValidationResult[] errors = new DataValidationResult[0];

        public DataValidationException()
        {
        }

        public DataValidationException(string message)
            : base(message)
        {
        }

        public DataValidationException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public DataValidationException(string message, IEnumerable<DataValidationResult> validationErrors)
            : this(message)
        {
            errors = validationErrors.ToArray();
        }

        public DataValidationException(string message, IEnumerable<DataValidationResult> validationErrors, Exception innerException)
            : this(message, innerException)
        {
            errors = validationErrors.ToArray();
        }

        protected DataValidationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            this.errors = (DataValidationResult[]) info.GetValue(ErrorsKey, typeof (DataValidationResult[]));
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(ErrorsKey, errors);
        }

        public IEnumerable<DataValidationResult> ValidationErrors
        {
            get { return errors; }
        }
    }

    [Serializable]
    public class DataValidationResult
    {
        private readonly List<ValidationError> errors;

        public DataValidationResult(IEntityEntry entry, IEnumerable<ValidationError> errors)
        {
            Entry = entry;
            this.errors = errors.ToList();
        }

        public IEntityEntry Entry { get; private set; }

        public IEnumerable<ValidationError> Errors
        {
            get { return errors; }
        }
    }

    [Serializable]
    public class ValidationError
    {
        public ValidationError(string propertyName, string errorMessage)
        {
            PropertyName = propertyName;
            ErrorMessage = errorMessage;
        }

        public string PropertyName { get; private set; }
        public string ErrorMessage { get; private set; }
    }
}