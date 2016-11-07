namespace Ergate.DataAccess.UnitTests.TestDoubles
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Ergate.DataAccess;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    class InterceptorDouble : IEntityInterceptor
    {
        private readonly List<IEntityEntry> onLoad = new List<IEntityEntry>();
        private readonly List<IEntityEntry> onSave = new List<IEntityEntry>();
        private readonly List<IEntityEntry> onDelete = new List<IEntityEntry>();

        public IEnumerable<object> InterceptedOnOnLoad
        {
            get { return this.onLoad.Select(e => e.Entity); }
        }

        public IEnumerable<object> InterceptedOnSave
        {
            get { return this.onSave.Select(e => e.Entity); }
        }

        public IEnumerable<object> InterceptedOnDelete
        {
            get { return this.onDelete.Select(e => e.Entity); }
        }

        public void OnLoad(IEntityEntry entry, IRepository repository)
        {
            this.onLoad.Add(entry);
        }

        public void OnSave(IEntityEntry entry, IUnitOfWork repository)
        {
            this.onSave.Add(entry);
        }

        public void OnDelete(IEntityEntry entry, IUnitOfWork repository)
        {
            this.onDelete.Add(entry);
        }
    }

    static class InterceptorDoubleExtensions
    {
        public static void AssertIntercepted<T>(this InterceptorDouble interceptor, Func<InterceptorDouble, IEnumerable<object>> onFunc, T[] entities, Func<T, string> msgFunc = null )
        {
            if (msgFunc == null)
                msgFunc = arg => arg.ToString();

            StringBuilder errors = new StringBuilder();
            for (int i = 0; i < entities.Length; i++)
            {
                if (!onFunc(interceptor).Contains(entities[i]))
                    errors.AppendLine(string.Format("User Name='{0}' was not intercepted", msgFunc(entities[i])));
            }
            if (errors.Length > 0)
                Assert.Fail(errors.ToString());
        }
    }
}