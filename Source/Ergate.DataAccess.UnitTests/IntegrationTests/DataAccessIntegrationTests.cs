namespace iQuarc.DataAccess.IntegrationTests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Reflection;

    using Ergate.DataAccess;
    using Ergate.DataAccess.AppBoot;
    using Ergate.Infrastructure.AppBoot;
    using Ergate.Infrastructure.AppBoot.Container;
    using Ergate.Infrastructure.Unity;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataAccessIntegrationTests
    {
        private Bootstrapper bootstrapper;

        [TestInitialize]
        public void Init()
        {
            var assemblies = this.GetAssemblies();

            this.bootstrapper = new Bootstrapper(assemblies);
            this.bootstrapper.ConfigureWithUnity();
            this.bootstrapper.AddRegistrationBehavior(new ServiceRegistrationBehavior());
            this.bootstrapper.AddRegistrationBehavior(DataAccessConfigurations.DefaultRegistrationConventions);

            this.bootstrapper.Run();
        }

        [TestMethod]
        public void TestContainerIntegration()
        {
            var repository = this.bootstrapper.ServiceLocator.GetInstance<IRepository>();
            Assert.IsNotNull(repository);
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (var dll in Directory.GetFiles(path, "*.dll"))
            {
                var filename = Path.GetFileName(dll);
                if (filename != null && filename.StartsWith("Ergate.DataAccess"))
                {
                    var assembly = Assembly.LoadFile(dll);
                    yield return assembly;
                }
            }
        }
    }

    [Service(typeof(IDbContextFactory))]
    public class DbContextFactory : DbContextFactory<DummyContext>
    {
    }

    public class DummyContext : DbContext
    {
    }

    [Service(typeof(IEntityInterceptor))]
    public class DummyInterceptor : IEntityInterceptor
    {
        public void OnDelete(IEntityEntry entry, IUnitOfWork unitOfWork)
        {
        }

        public void OnLoad(IEntityEntry entry, IRepository repository)
        {
        }

        public void OnSave(IEntityEntry entry, IUnitOfWork unitOfWork)
        {
        }
    }
}