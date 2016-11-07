namespace Ergate.DataAccess.UnitTests.TestDoubles
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using Ergate.DataAccess;

    class FakeSet<T> : DbSet<T>, IQueryable where T : class
    {
        private readonly DbContextFakeWrapper wrapper;
        private readonly List<T> values = new List<T>();

        private IQueryable<T> queryable; 

        private IQueryable<T> Queryable
        {
            get
            {
                if (this.queryable == null)
                    this.queryable = this.EnumerateAndRaiseEvent().AsQueryable();
                return this.queryable;
            }
        }

        private IEnumerable<T> EnumerateAndRaiseEvent()
        {
            foreach (var value in this.values)
            {
                this.RaiseEntityLoaded(value);
                yield return value;
            }
        }

        public FakeSet()
        {
            this.values = new List<T>();
        }

        public FakeSet(IEnumerable<T> values)
            : this(values, null)
        {
        }

        public FakeSet(IEnumerable<T> values, DbContextFakeWrapper wrapper)
        {
            this.wrapper = wrapper;
            this.values.AddRange(values);
        }

        IQueryProvider IQueryable.Provider
        {
            get { return this.Queryable.Provider; }
        }

        Expression IQueryable.Expression
        {
            get { return this.Queryable.Expression; }
        }

        Type IQueryable.ElementType
        {
            get { return this.Queryable.ElementType; }
        }

        private void RaiseEntityLoaded(T value)
        {
            if (this.wrapper != null)
            {
                this.wrapper.RaiseEntityLoaded(new EntityLoadedEventHandlerArgs(value));
            }
        }

        public IList<T> Values
        {
            get { return this.values; }
        }

        public override T Add(T entity)
        {
            this.values.Add(entity);
            return entity;
        }

        public override T Remove(T entity)
        {
            this.values.Remove(entity);
            return entity;
        }
    }
}