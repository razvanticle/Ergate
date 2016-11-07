namespace Ergate.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Linq.Expressions;

    using Ergate.DataAccess;
    using Ergate.DataAccess.ExceptionHandling;
    using Ergate.DataAccess.UnitTests.TestDoubles;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
	public class UnitOfWorkSaveTests
	{
		[TestMethod]
		public void SaveChanges_ModifiedEntitiesAndGlobalInterceptors_EntitiesIntercepted()
		{
			this.ParamTest__ModifiedEntities_EntitiesIntercepted(
				resolver => resolver.GetGlobalInterceptors());
		}

		[TestMethod]
		public void SaveChanges_ModifiedEntitiesAndEntityInterceptors_EntitiesIntercepted()
		{
			this.ParamTest__ModifiedEntities_EntitiesIntercepted(
				resolver => resolver.GetEntityInterceptors(It.IsAny<Type>()));
		}

		private void ParamTest__ModifiedEntities_EntitiesIntercepted(Expression<Func<IInterceptorsResolver, IEnumerable<IEntityInterceptor>>> getInterceptorsFunction)
		{
			User user1 = new User(1, "John");
			User user2 = new User(2, "Mary");

			InterceptorDouble interceptorMock = new InterceptorDouble();

			Mock<IInterceptorsResolver> interceptorResolverStub = new Mock<IInterceptorsResolver>();
			interceptorResolverStub.Setup(getInterceptorsFunction).Returns(new[] {interceptorMock});

			UnitOfWork uof = this.GetTargetWith(new[] {user1, user2}, interceptorResolverStub.Object);

			uof.SaveChanges();

			AssertInterceptedOnSave(interceptorMock, user1, user2);
		}

		[TestMethod]
		public void SaveChanges_DeletedEntitiesAndGlobalInterceptors_DeletedEntitiesIntercepted()
		{
			this.ParamTest__DeletedEntities_DeletedEntitiesIntercepted(
				i => i.GetGlobalInterceptors());
		}

		[TestMethod]
		public void SaveChanges_DeletedEntitiesAndEntityInterceptors_DeletedEntitiesIntercepted()
		{
			this.ParamTest__DeletedEntities_DeletedEntitiesIntercepted(
				i => i.GetEntityInterceptors(It.IsAny<Type>()));
		}

		private void ParamTest__DeletedEntities_DeletedEntitiesIntercepted(Expression<Func<IInterceptorsResolver, IEnumerable<IEntityInterceptor>>> getInterceptorsFunction)
		{
			InterceptorDouble interceptorMock = new InterceptorDouble();
			Mock<IInterceptorsResolver> resolverMock = new Mock<IInterceptorsResolver>();
			resolverMock.Setup(getInterceptorsFunction).Returns(new[] {interceptorMock});

			User u1 = new User(1, "John");
			User u2 = new User(2, "Mary");

			ContextUtilitiesDouble contextStub = new ContextUtilitiesDouble(new[] {u1, u2}, EntityEntryState.Deleted);
			UnitOfWork uof = this.GetTargetWith(contextStub, resolverMock.Object);

			uof.SaveChanges();

			this.AssertInterceptedOnDelete(interceptorMock, u1, u2);
		}


		[TestMethod]
		public void SaveChanges_OnSaveModifiesNewEntitiesAndGlobalInterceptor_NewModifiedEntityAlsoIntercepted()
		{
			this.ParamTest__OnSaveModifiesOtherEntities_NewModifiedEntitiesAlsoIntercepted(
				resolver => resolver.GetGlobalInterceptors());
		}

		[TestMethod]
		public void SaveChanges_OnSaveModifiesNewEntitiesAndEntityInterceptors_NewModifiedEntityAlsoIntercepted()
		{
			this.ParamTest__OnSaveModifiesOtherEntities_NewModifiedEntitiesAlsoIntercepted(
				resolver => resolver.GetEntityInterceptors(It.IsAny<Type>()));
		}

		private void ParamTest__OnSaveModifiesOtherEntities_NewModifiedEntitiesAlsoIntercepted(
			Expression<Func<IInterceptorsResolver, IEnumerable<IEntityInterceptor>>> getInterceptorsFunction)
		{
			User user1 = new User(1, "John");
			User user2 = new User(2, "Mary");
			ContextUtilitiesDouble contextStub = new ContextUtilitiesDouble();
			contextStub.AddEntitiesByCallNumber(1, new[] {user1}); // 1st call to GetChangedEntities --> user1
			contextStub.AddEntitiesByCallNumber(2, new[] {user1, user2}); // 2nd call to GetChangedEntities --> user1 and user2

			InterceptorDouble interceptorMock = new InterceptorDouble();

			Mock<IInterceptorsResolver> interceptorResolverStub = new Mock<IInterceptorsResolver>();
			interceptorResolverStub.Setup(getInterceptorsFunction).Returns(new[] {interceptorMock});


			UnitOfWork uof = this.GetTargetWith(contextStub, interceptorResolverStub.Object);


			uof.SaveChanges();

			AssertInterceptedOnSave(interceptorMock, user1, user2);
		}

		[TestMethod]
		public void SaveChanges_OnSaveDeletesEntitiesAndGlobalInterceptor_DeletedEntitiesIntercepted()
		{
			this.ParamTest__OnSaveDeletesEntities_DeletedEntitiesAlsoIntercepted(
				resolver => resolver.GetGlobalInterceptors());
		}

		[TestMethod]
		public void SaveChanges_OnSaveDeletesEntitiesAndEntityInterceptors_DeletedEntitiesIntercepted()
		{
			this.ParamTest__OnSaveDeletesEntities_DeletedEntitiesAlsoIntercepted(
				resolver => resolver.GetEntityInterceptors(It.IsAny<Type>()));
		}

		private void ParamTest__OnSaveDeletesEntities_DeletedEntitiesAlsoIntercepted(
			Expression<Func<IInterceptorsResolver, IEnumerable<IEntityInterceptor>>> getInterceptorsFunction)
		{
			var u1 = new User(1, "John").AsEntry();
			var u2 = new User(2, "Mary").AsEntry(EntityEntryState.Deleted);
			ContextUtilitiesDouble contextStub = new ContextUtilitiesDouble();
			contextStub.AddEntriesByCallNumber(1, new[] { u1 }); // 1st call to GetChangedEntities --> modified entity
			contextStub.AddEntriesByCallNumber(2, new[] { u1, u2 }); // 2nd call to GetChangedEntities --> modified and deleted entities

			InterceptorDouble interceptorMock = new InterceptorDouble();

			Mock<IInterceptorsResolver> interceptorResolverStub = new Mock<IInterceptorsResolver>();
			interceptorResolverStub.Setup(getInterceptorsFunction).Returns(new[] { interceptorMock });

			UnitOfWork uof = this.GetTargetWith(contextStub, interceptorResolverStub.Object);


			uof.SaveChanges();

			this.AssertInterceptedOnDelete(interceptorMock, u2.Entity as User);
		}

		[TestMethod]
		public void SaveChanges_OnDeleteDeletesEntitiesAndGlobalInterceptor_DeletedEntitiesIntercepted()
		{
			this.ParamTest__OnDeleteDeletesEntities_DeletedEntitiesAlsoIntercepted(
				resolver => resolver.GetGlobalInterceptors());
		}

		[TestMethod]
		public void SaveChanges_OnDeleteDeletesEntitiesAndEntityInterceptors_DeletedEntitiesIntercepted()
		{
			this.ParamTest__OnDeleteDeletesEntities_DeletedEntitiesAlsoIntercepted(
				resolver => resolver.GetEntityInterceptors(It.IsAny<Type>()));
		}

		private void ParamTest__OnDeleteDeletesEntities_DeletedEntitiesAlsoIntercepted(
			Expression<Func<IInterceptorsResolver, IEnumerable<IEntityInterceptor>>> getInterceptorsFunction)
		{
			var u1 = new User(1, "John").AsEntry(EntityEntryState.Deleted);
			var u2 = new User(2, "Mary").AsEntry(EntityEntryState.Deleted);
			ContextUtilitiesDouble contextStub = new ContextUtilitiesDouble();
			contextStub.AddEntriesByCallNumber(1, new[] { u1 }); // 1st call to GetChangedEntities --> one deleted entity
			contextStub.AddEntriesByCallNumber(2, new[] { u1, u2 }); // 2nd call to GetChangedEntities --> two deleted entities

			InterceptorDouble interceptorMock = new InterceptorDouble();

			Mock<IInterceptorsResolver> interceptorResolverStub = new Mock<IInterceptorsResolver>();
			interceptorResolverStub.Setup(getInterceptorsFunction).Returns(new[] { interceptorMock });

			UnitOfWork uof = this.GetTargetWith(contextStub, interceptorResolverStub.Object);


			uof.SaveChanges();

			this.AssertInterceptedOnDelete(interceptorMock, u1.Entity as User, u2.Entity as User);
		}

		[TestMethod]
		public void SaveChanges_OnDeleteModifiesEntitiesAndGlobalInterceptor_ModifiedEntitiesIntercepted()
		{
			this.ParamTest__OnDeleteModifiesEntities_ModifiedEntitiesAlsoIntercepted(
				resolver => resolver.GetGlobalInterceptors());
		}

		[TestMethod]
		public void SaveChanges_OnDeleteModifiesEntitiesAndEntityInterceptors_ModifiedEntitiesIntercepted()
		{
			this.ParamTest__OnDeleteModifiesEntities_ModifiedEntitiesAlsoIntercepted(
				resolver => resolver.GetEntityInterceptors(It.IsAny<Type>()));
		}

		private void ParamTest__OnDeleteModifiesEntities_ModifiedEntitiesAlsoIntercepted(
			Expression<Func<IInterceptorsResolver, IEnumerable<IEntityInterceptor>>> getInterceptorsFunction)
		{
			var u1 = new User(1, "John").AsEntry(EntityEntryState.Deleted);
			var u2 = new User(2, "Mary").AsEntry(EntityEntryState.Modified);
			ContextUtilitiesDouble contextStub = new ContextUtilitiesDouble();
			contextStub.AddEntriesByCallNumber(1, new[] { u1 }); // 1st call to GetChangedEntities --> one deleted entity
			contextStub.AddEntriesByCallNumber(2, new[] { u1, u2 }); // 2nd call to GetChangedEntities --> deleted and modified entities

			InterceptorDouble interceptorMock = new InterceptorDouble();

			Mock<IInterceptorsResolver> interceptorResolverStub = new Mock<IInterceptorsResolver>();
			interceptorResolverStub.Setup(getInterceptorsFunction).Returns(new[] { interceptorMock });

			UnitOfWork uof = this.GetTargetWith(contextStub, interceptorResolverStub.Object);


			uof.SaveChanges();

			AssertInterceptedOnSave(interceptorMock, u2.Entity as User);
		}

		private static void AssertInterceptedOnSave(InterceptorDouble interceptorMock, params User[] users)
		{
			interceptorMock.AssertIntercepted(d => d.InterceptedOnSave, users, u => u.Name);
		}

		private void AssertInterceptedOnDelete(InterceptorDouble interceptorMock, params User[] users)
		{
			interceptorMock.AssertIntercepted(d => d.InterceptedOnDelete, users, u => u.Name);
		}

		[TestMethod]
		public void SaveChanges_WhenCalled_SaveChangesOnContext()
		{
			Mock<DbContext> contextMock = new Mock<DbContext>();
			UnitOfWork uof = this.GetTargetWith(contextMock);

			uof.SaveChanges();

			contextMock.Verify(c => c.SaveChanges(), Times.AtLeastOnce);
		}

		[TestMethod]
		public void SaveChanges_InterceptorThrowsException_ExceptionHandlerGetsIt()
		{
			Exception e = new Exception();
			Mock<IEntityInterceptor> interceptorStub = new Mock<IEntityInterceptor>();
			interceptorStub.Setup(i => i.OnSave(It.IsAny<IEntityEntry>(), It.IsAny<IUnitOfWork>()))
			               .Throws(e);

			FakeExceptionHandler handler = new FakeExceptionHandler();
			UnitOfWork uof = this.GetTargetWith(interceptorStub.Object, handler);

			uof.SaveChanges();

			Assert.AreSame(e, handler.Handled);
		}

		[TestMethod]
		public void SaveChanges_ContextSaveThrowsException_ExceptionHandlerGetsIt()
		{
			Exception e = new Exception();
			Mock<DbContext> dbContextStub = new Mock<DbContext>();
			dbContextStub.Setup(c => c.SaveChanges()).Throws(e);

			FakeExceptionHandler handler = new FakeExceptionHandler();
			UnitOfWork uof = this.GetTargetWith(dbContextStub, handler);

			uof.SaveChanges();

			Assert.AreSame(e, handler.Handled);
		}

		private UnitOfWork GetTargetWith(Mock<DbContext> context, FakeExceptionHandler handler)
		{
			ContextUtilitiesDouble utilitiesStub = new ContextUtilitiesDouble(new[] {new User()});
			Mock<IInterceptorsResolver> resolverStub = new Mock<IInterceptorsResolver>();

			return this.GetTargetWith(utilitiesStub, resolverStub.Object, context, handler);
		}

		private UnitOfWork GetTargetWith(IEntityInterceptor interceptor, IExceptionHandler handler)
		{
			ContextUtilitiesDouble utilitiesStub = new ContextUtilitiesDouble(new[] {new User()});

			Mock<IInterceptorsResolver> resolverStub = new Mock<IInterceptorsResolver>();
			resolverStub.Setup(r => r.GetGlobalInterceptors()).Returns(new[] {interceptor});

			return this.GetTargetWith(utilitiesStub, resolverStub.Object, new Mock<DbContext>(), handler);
		}

		private UnitOfWork GetTargetWith(Mock<DbContext> context)
		{
			ContextUtilitiesDouble contextUtilitiesStub = new ContextUtilitiesDouble(Enumerable.Empty<object>());
			return this.GetTargetWith(contextUtilitiesStub, new Mock<IInterceptorsResolver>().Object, context);
		}

		private UnitOfWork GetTargetWith(IEnumerable<object> changedEntities, IInterceptorsResolver interceptorsResolver)
		{
			ContextUtilitiesDouble contextUtilitiesStub = new ContextUtilitiesDouble(changedEntities);
			return this.GetTargetWith(contextUtilitiesStub, interceptorsResolver, new Mock<DbContext>());
		}

		private UnitOfWork GetTargetWith(IDbContextUtilities contextUtilities, IInterceptorsResolver interceptorsResolver)
		{
			return this.GetTargetWith(contextUtilities, interceptorsResolver, new Mock<DbContext>());
		}

		private UnitOfWork GetTargetWith(IDbContextUtilities contextUtilitiesStub, IInterceptorsResolver interceptorsResolver, Mock<DbContext> contextStub)
		{
			IExceptionHandler handler = new Mock<IExceptionHandler>().Object;
			return this.GetTargetWith(contextUtilitiesStub, interceptorsResolver, contextStub, handler);
		}

		private UnitOfWork GetTargetWith(IDbContextUtilities contextUtilitiesStub, IInterceptorsResolver interceptorsResolver, Mock<DbContext> contextStub, IExceptionHandler handler)
		{
			var contextFactoryStub = contextStub.BuildFactoryStub();
			return new UnitOfWork(contextFactoryStub, interceptorsResolver, contextUtilitiesStub, handler);
		}


		private class User
		{
			public User()
			{
			}

			public User(int id, string name)
			{
				this.Id = id;
				this.Name = name;
			}

			public int Id { get; set; }
			public string Name { get; set; }
		}
	}
}