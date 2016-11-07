namespace Ergate.Infrastructure.UnitTests.AppBoot
{
    using System.Linq;

    using Ergate.Infrastructure.AppBoot;
    using Ergate.Infrastructure.AppBoot.Container;

    using iQuarc.xUnitEx;

    using Xunit;

    /// <summary>
    /// Test cases take all relevant combinations between:
    ///     FromType        Contract        Priority    --> Expected Result
    ///     Different        Same            Same       -->     All
    ///     Same             Same           Same        -->     One
    ///     . . .
    /// <para>
    ///    Registration catalog can contain:
    ///         1. For one FromType it has registrations given by ONLY one behavior, and that behavior is the one with the highest priority
    ///         2. Registrations with same FromType and same Contract are ignored or overwritten
    /// </para>
    /// </summary>
    public class RegistrationsCatalogTests
    {
        private RegistrationsCatalog catalog;

        public RegistrationsCatalogTests()
        {
            this.catalog = new RegistrationsCatalog();
        }

        [Fact]
        public void Add__DifferentFrom_NullContract_SamePriority__AllAdded()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<int>(null);
            ServiceInfo si3 = GetSi<byte>(null);

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2, si3);
        }

        [Fact]
        public void Add__SameFrom_NullContract_HigherPriorityAddedLater__HigherPriorityOverwrites()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<string>(null);
            ServiceInfo si3 = GetSi<string>(null);

            this.catalog.Add(si1, 2);
            this.catalog.Add(si2, 3);
            this.catalog.Add(si3, 5);

            this.AssertCatalogContainsOnly(si3);
        }

        [Fact]
        public void Add__SameFrom_NullContract_LowerPriorityAddedLater__HigherPriorityIsNotOverwritten()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<string>(null);
            ServiceInfo si3 = GetSi<string>(null);

            this.catalog.Add(si2, 5);
            this.catalog.Add(si1, 2);
            this.catalog.Add(si3, 1);

            this.AssertCatalogContainsOnly(si2);
        }

        [Fact]
        public void Add__DifferentFrom_SameContract_SamePriority__AllAdded()
        {
            ServiceInfo si1 = GetSi<string>("Contract");
            ServiceInfo si2 = GetSi<int>("Contract");
            ServiceInfo si3 = GetSi<byte>("Contract");

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2, si3);
        }

        [Fact]
        public void Add__SameFrom_SameContract_SamePriority__OnlyOneAdded()
        {
            ServiceInfo si1 = GetSi<string>("Contract");
            ServiceInfo si2 = GetSi<string>("Contract");
            ServiceInfo si3 = GetSi<string>("Contract");

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 1);

            Assert.Equal(1, this.catalog.Count());
        }

        [Fact]
        public void Add_SameFrom_DifferentContract_SamePriority__AllAdded()
        {
            ServiceInfo si1 = GetSi<string>("Contract1");
            ServiceInfo si2 = GetSi<string>("Contract2");
            ServiceInfo si3 = GetSi<string>("Contract3");

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2, si3);
        }

        [Fact]
        public void Add__SameFrom_DifferentContract_HighPriorityAddedLast__LowerPrioritiesOverwritten()
        {
            ServiceInfo si1 = GetSi<string>("Contract1");
            ServiceInfo si2 = GetSi<string>("Contract2");
            ServiceInfo si3 = GetSi<string>("Contract3");

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 3);

            this.AssertCatalogContainsOnly(si3);
        }

        [Fact]
        public void Add__SameFrom_SameContractForDifferentPrio_HighPriorityAddedLast__LowerPrioritiesOverwritten()
        {
            ServiceInfo si1 = GetSi<string>("Contract1");
            ServiceInfo si2 = GetSi<string>("Contract2");
            ServiceInfo si3 = GetSi<string>("Contract1");

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 3);

            this.AssertCatalogContainsOnly(si3);
        }

        [Fact]
        public void Add__SameFrom_DifferentContract_HighPriorityAddedFirst__HighPrioritiesRemain()
        {
            ServiceInfo si1 = GetSi<string>("Contract1");
            ServiceInfo si2 = GetSi<string>("Contract2");
            ServiceInfo si3 = GetSi<string>("Contract3");

            this.catalog.Add(si1, 3);
            this.catalog.Add(si2, 3);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2);
        }

        [Fact]
        public void Add__SameFrom_SameContract_HighPriorityAddedFirst__HighPrioritiesRemain()
        {
            ServiceInfo si1 = GetSi<string>("Contract");
            ServiceInfo si2 = GetSi<string>("Contract2");
            ServiceInfo si3 = GetSi<string>("Contract");

            this.catalog.Add(si1, 3);
            this.catalog.Add(si2, 3);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2);
        }

        [Fact]
        public void Add__SameType_NullAndContracts_SamePriority__AllAdded()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<string>("Contract1");
            ServiceInfo si3 = GetSi<string>("Contract2");

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2, si3);
        }

        [Fact]
        public void Add__SameFrom_NullAndContracts_HighPriorityAddedLast__LowerPrioritiesOverwritten()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<string>("Contract2");
            ServiceInfo si3 = GetSi<string>("Contract3");

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 3);

            this.AssertCatalogContainsOnly(si3);
        }

        [Fact]
        public void Add__SameFrom_NullAndContract_HighPriorityAddedFirst__HighPrioritiesRemain()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<string>("Contract2");
            ServiceInfo si3 = GetSi<string>("Contract3");

            this.catalog.Add(si1, 3);
            this.catalog.Add(si2, 3);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2);
        }

        [Fact]
        public void Add__SameFrom_SameNullAndContract_HighPriorityAddedLast__LowerPrioritiesOverwritten()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<string>("Contract");
            ServiceInfo si3 = GetSi<string>(null);

            this.catalog.Add(si1, 1);
            this.catalog.Add(si2, 1);
            this.catalog.Add(si3, 3);

            this.AssertCatalogContainsOnly(si3);
        }

        [Fact]
        public void Add__SameFrom_SameNullAndContract_HighPriorityAddedFirst__HighPrioritiesRemain()
        {
            ServiceInfo si1 = GetSi<string>(null);
            ServiceInfo si2 = GetSi<string>("Contract");
            ServiceInfo si3 = GetSi<string>(null);

            this.catalog.Add(si1, 3);
            this.catalog.Add(si2, 3);
            this.catalog.Add(si3, 1);

            AssertEx.AreEquivalent(this.catalog, si1, si2);
        }

        private static ServiceInfo GetSi<TFrom>(string contractName)
        {
            return new ServiceInfo(typeof (TFrom), typeof (int), contractName, Lifetime.Instance);
        }

        private void AssertCatalogContainsOnly(ServiceInfo si)
        {
            ServiceInfo[] catalogAsArray = this.catalog.ToArray();
            Assert.True(catalogAsArray.Length == 1, "Catalog contains zero or more registrations, but one expected");
            Assert.Same(si, catalogAsArray[0]);
            
        }
    }
}