﻿namespace Ergate.Common.Extensions
{
    using System;

    public static class ExceptionExtensions
    {
        public static T FirstInner<T>(this Exception exception) where T : Exception
        {
            if (exception == null)
                return null;

            if (exception is T)
                return (T)exception;

            return exception.InnerException.FirstInner<T>();
        }

        public static Exception InnerMostException(this Exception exception)
        {
            if (exception == null)
                return null;

            if (exception.InnerException == null)
                return exception;

            return exception.InnerException.InnerMostException();
        }
    }
}