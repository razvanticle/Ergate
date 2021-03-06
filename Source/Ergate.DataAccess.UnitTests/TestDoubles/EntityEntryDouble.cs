namespace Ergate.DataAccess.UnitTests.TestDoubles
{
    using System;
    using System.Collections.Generic;

    using Ergate.DataAccess;

    class EntityEntryDouble : IEntityEntry
	{
		public EntityEntryDouble()
		{
		}

		public EntityEntryDouble(object entity, EntityEntryState state)
		{
			this.Entity = entity;
			this.State = state;
		}

		public object Entity { get; private set; }
		public EntityEntryState State { get; set; }
		public object GetOriginalValue(string propertyName)
		{
			throw new NotImplementedException();
		}

	    public object GetCurrentValue(string propertyName)
	    {
	        throw new NotImplementedException();
	    }

	    public IEntityEntry<T> Convert<T>() where T : class
		{
			throw new NotImplementedException();
		}

		public void SetOriginalValue(string propertyName, object value)
		{
			throw new NotImplementedException();
		}

	    public void Reload()
	    {
	        throw new NotImplementedException();
	    }

	    public IEnumerable<string> GetProperties()
	    {
	        throw new NotImplementedException();
	    }

	    public IPropertyEntry Property(string name)
	    {
	        throw new NotImplementedException();
	    }

	    protected bool Equals(EntityEntryDouble other)
		{
			return Equals(this.Entity, other.Entity) && this.State == other.State;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (ReferenceEquals(this, obj)) return true;
			if (obj.GetType() != this.GetType()) return false;
			return this.Equals((EntityEntryDouble) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				return ((this.Entity != null ? this.Entity.GetHashCode() : 0)*397) ^ (int) this.State;
			}
		}
	}

    class EntityEntryDouble<T> : IEntityEntry<T> where T : class
    {
        public EntityEntryDouble()
        {
        }

        public EntityEntryDouble(T entity, EntityEntryState state)
        {
            this.Entity = entity;
            this.State = state;
        }

        public T Entity { get; set; }
        public EntityEntryState State { get; set; }
        public object GetOriginalValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public object GetCurrentValue(string propertyName)
        {
            throw new NotImplementedException();
        }

        public void SetOriginalValue(string propertyName, object value)
        {
            throw new NotImplementedException();
        }

        public void Reload()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetProperties()
        {
            throw new NotImplementedException();
        }

        public IPropertyEntry Property(string name)
        {
            throw new NotImplementedException();
        }

        protected bool Equals(EntityEntryDouble<T> other)
	    {
		    return EqualityComparer<T>.Default.Equals(this.Entity, other.Entity) && this.State == other.State;
	    }

	    public override bool Equals(object obj)
	    {
		    if (ReferenceEquals(null, obj)) return false;
		    if (ReferenceEquals(this, obj)) return true;
		    if (obj.GetType() != this.GetType()) return false;
		    return this.Equals((EntityEntryDouble<T>) obj);
	    }

	    public override int GetHashCode()
	    {
		    unchecked
		    {
			    return (EqualityComparer<T>.Default.GetHashCode(this.Entity)*397) ^ (int) this.State;
		    }
	    }
    }
}