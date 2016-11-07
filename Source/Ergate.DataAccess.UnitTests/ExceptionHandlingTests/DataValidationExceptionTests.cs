namespace Ergate.DataAccess.UnitTests.ExceptionHandlingTests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;

    using Ergate.DataAccess;
    using Ergate.DataAccess.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataValidationExceptionTests
    {
        [TestMethod]
        public void Serialize_MoreEntitiesWithMoreErrors_DeserializedIsSame()
        {
            var errors1 = new[]
            {
                new ValidationError("UserName", "error1"),
                new ValidationError("UserEmail", "error2")
            };
            var entityErrors1 = new DataValidationResult(new EntryDouble(new User(1)), errors1);
            var errors2 = new[]
            {
                new ValidationError("RoleName", "error3"),
                new ValidationError("AccessRights", "error4")
            };
            var entityErrors2 = new DataValidationResult(new EntryDouble(new Role(2)), errors2);

            DataValidationException e = new DataValidationException(string.Empty, new[] {entityErrors1, entityErrors2});

            using (Stream s = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(s, e);
                s.Position = 0; // Reset stream position
                e = (DataValidationException) formatter.Deserialize(s);
            }

            DataValidationResult[] actual = e.ValidationErrors.ToArray();
            this.AssertAreEqual(entityErrors1, actual[0]);
            this.AssertAreEqual(entityErrors2, actual[1]);
        }

        private void AssertAreEqual(DataValidationResult expected, DataValidationResult actual)
        {
            Func<ValidationError, ValidationError, bool> equalityFunc = (e1, e2) => e1.ErrorMessage == e2.ErrorMessage && e1.PropertyName == e2.PropertyName;

            Assert.AreEqual(expected.Entry, actual.Entry);
            AssertEx.AreEquivalent(actual.Errors, equalityFunc, expected.Errors.ToArray());
        }

        [Serializable]
        private class EntryDouble : IEntityEntry
        {
            public EntryDouble(object entity)
            {
                this.Entity = entity;
            }

            public object Entity { get; set; }

            public EntityEntryState State
            {
                get { throw new NotImplementedException(); }
                set { }
            }

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

            protected bool Equals(EntryDouble other)
            {
                return Equals(this.Entity, other.Entity);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((EntryDouble) obj);
            }

            public override int GetHashCode()
            {
                return (this.Entity != null ? this.Entity.GetHashCode() : 0);
            }
        }

        [Serializable]
        private class User
        {
            public User(int id)
            {
                this.Id = id;
            }

            public int Id { get; set; }

            protected bool Equals(User other)
            {
                return this.Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((User) obj);
            }

            public override int GetHashCode()
            {
                return this.Id;
            }
        }

        [Serializable]
        private class Role
        {
            public Role(int id)
            {
                this.Id = id;
            }

            public int Id { get; set; }

            protected bool Equals(Role other)
            {
                return this.Id == other.Id;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return this.Equals((Role) obj);
            }

            public override int GetHashCode()
            {
                return this.Id;
            }
        }
    }
}