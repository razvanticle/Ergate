using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using iQuarc.DataAccess.AppBoot;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace iQuarc.DataAccess.IntegrationTests
{
    using Ergate.Infrastructure.AppBoot;
    using Ergate.Infrastructure.AppBoot.Container;
    using Ergate.Infrastructure.Unity;

    [TestClass]
    public class DataAccessIntegrationTests
    {
        private Bootstrapper bootstrapper;
        
        [TestInitialize]
        public void Init()
        {
            IEnumerable<Assembly> assemblies = GetAssemblies();

            bootstrapper = new Bootstrapper(assemblies);
            bootstrapper.ConfigureWithUnity();
            bootstrapper.AddRegistrationBehavior(new ServiceRegistrationBehavior());
            bootstrapper.AddRegistrationBehavior(DataAccessConfigurations.DefaultRegistrationConventions);

            bootstrapper.Run();
        }

        [TestMethod]
        public void TestContainerIntegration()
        {
            var repository = bootstrapper.ServiceLocator.GetInstance<IRepository>();
            Assert.IsNotNull(repository);
        }

        private IEnumerable<Assembly> GetAssemblies()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            foreach (string dll in Directory.GetFiles(path, "*.dll"))
            {
                string filename = Path.GetFileName(dll);
                if (filename != null && (filename.StartsWith("iQuarc.DataAccess")))
                {
                    Assembly assembly = Assembly.LoadFile(dll);
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
        public void OnLoad(IEntityEntry entry, IRepository repository)
        {
            
        }

        public void OnSave(IEntityEntry entry, IUnitOfWork unitOfWork)
        {
            
        }

        public void OnDelete(IEntityEntry entry, IUnitOfWork unitOfWork)
        {
           
        }
    }
}
