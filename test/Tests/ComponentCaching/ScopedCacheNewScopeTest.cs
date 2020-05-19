using ComposerCore.Implementation;
using ComposerCore.Tests.ComponentCaching.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ComponentCaching
{
    [TestClass]
    public class ScopedCacheNewScopeTest
    {
        private ComponentContext _context;

        #region Additional test attributes
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _context = new ComponentContext();
            _context.Register(typeof(ScopedComponent));
            _context.Register(typeof(ScopedComponentWithPlugs));
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void SameScopeResolvesSameObjects()
        {
            var scope = _context.CreateScope();

            var c1 = scope.GetComponent<ScopedComponent>();
            var c2 = scope.GetComponent<ScopedComponent>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreSame(c1, c2);
        }

        [TestMethod]
        public void DifferentScopesResolveDifferentObjects()
        {
            var c1 = _context.CreateScope().GetComponent<ScopedComponent>();
            var c2 = _context.CreateScope().GetComponent<ScopedComponent>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreNotSame(c1, c2);
        }

        [TestMethod]
        public void ScopeAndRootResolveToDifferentInstances()
        {
            var scope = _context.CreateScope();

            var c1 = _context.GetComponent<ScopedComponent>();
            var c2 = scope.GetComponent<ScopedComponent>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreNotSame(c1, c2);
        }

        [TestMethod]
        public void ScopedComponentPlugSameScope()
        {
            _context.Register(typeof(UncachedComponentWithPlugs));
            _context.Register(typeof(ContractAgnosticComponent));
            _context.Register(typeof(DefaultCacheComponent));
            _context.Register(typeof(UncachedComponent));

            var scope = _context.CreateScope();

            var c1 = scope.GetComponent<UncachedComponentWithPlugs>();
            var c2 = scope.GetComponent<UncachedComponentWithPlugs>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            
            Assert.IsNotNull(c1.ScopedComponent);
            Assert.IsNotNull(c2.ScopedComponent);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreSame(c1.ScopedComponent, c2.ScopedComponent);
        }

        [TestMethod]
        public void ScopedComponentPlugDifferentScope()
        {
            _context.Register(typeof(UncachedComponentWithPlugs));
            _context.Register(typeof(ContractAgnosticComponent));
            _context.Register(typeof(DefaultCacheComponent));
            _context.Register(typeof(UncachedComponent));

            var scope1 = _context.CreateScope();
            var scope2 = _context.CreateScope();

            var c1 = scope1.GetComponent<UncachedComponentWithPlugs>();
            var c2 = scope2.GetComponent<UncachedComponentWithPlugs>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            
            Assert.IsNotNull(c1.ScopedComponent);
            Assert.IsNotNull(c2.ScopedComponent);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c1.ScopedComponent, c2.ScopedComponent);
        }

        [TestMethod]
        public void ComponentPlugsOfScopedComponent()
        {
            _context.Register(typeof(ContractAgnosticComponent));
            _context.Register(typeof(DefaultCacheComponent));
            _context.Register(typeof(UncachedComponent));

            var scope1 = _context.CreateScope();
            var scope2 = _context.CreateScope();

            var pScoped = _context.GetComponent<ScopedComponent>();
            var pContractAgnostic = _context.GetComponent<ContractAgnosticComponent>();
            var pDefault = _context.GetComponent<DefaultCacheComponent>();
            var pUncached = _context.GetComponent<UncachedComponent>();
            
            Assert.IsNotNull(pScoped);
            Assert.IsNotNull(pContractAgnostic);
            Assert.IsNotNull(pDefault);
            Assert.IsNotNull(pUncached);
            
            var c11 = scope1.GetComponent<ScopedComponentWithPlugs>();
            var c12 = scope1.GetComponent<ScopedComponentWithPlugs>();
            var c2 = scope2.GetComponent<ScopedComponentWithPlugs>();
            
            Assert.IsNotNull(c11);
            Assert.IsNotNull(c12);
            Assert.IsNotNull(c2);
            
            Assert.AreSame(c11, c12);
            Assert.AreNotSame(c11, c2);

            Assert.AreNotSame(c11.ScopedComponent, c2.ScopedComponent);
            Assert.AreSame(c11.ContractAgnosticComponent, c2.ContractAgnosticComponent);
            Assert.AreSame(c11.DefaultCacheComponent, c2.DefaultCacheComponent);
            Assert.AreNotSame(c11.UncachedComponent, c2.UncachedComponent);
            
            Assert.AreNotSame(c11.ScopedComponent, pScoped);
            Assert.AreSame(c11.ContractAgnosticComponent, pContractAgnostic);
            Assert.AreSame(c11.DefaultCacheComponent, pDefault);
            Assert.AreNotSame(c11.UncachedComponent, pUncached);
        }

        [TestMethod]
        public void HierarchicalScopeForwardQueryOrder()
        {
            var scope1 = _context.CreateScope();
            var scope2 = scope1.CreateScope();
            var scope3 = scope2.CreateScope();

            var c1 = scope1.GetComponent<ScopedComponent>();
            var c2 = scope2.GetComponent<ScopedComponent>();
            var c3 = scope3.GetComponent<ScopedComponent>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c1, c3);
            Assert.AreNotSame(c2, c3);
        }
        
        [TestMethod]
        public void HierarchicalScopeReverseQueryOrder()
        {
            var scope1 = _context.CreateScope();
            var scope2 = scope1.CreateScope();
            var scope3 = scope2.CreateScope();

            var c1 = scope3.GetComponent<ScopedComponent>();
            var c2 = scope2.GetComponent<ScopedComponent>();
            var c3 = scope1.GetComponent<ScopedComponent>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c1, c3);
            Assert.AreNotSame(c2, c3);
        }
        
        [TestMethod]
        public void ScopeIsInjectedToScopedComponents()
        {
            _context.Register(typeof(ContractAgnosticComponent));
            _context.Register(typeof(DefaultCacheComponent));
            _context.Register(typeof(UncachedComponent));
            
            var scope1 = _context.CreateScope();
            var scope2 = scope1.CreateScope();
            var scope3 = scope2.CreateScope();

            var c0 = _context.GetComponent<ScopedComponentWithPlugs>();
            var c1 = scope1.GetComponent<ScopedComponentWithPlugs>();
            var c2 = scope2.GetComponent<ScopedComponentWithPlugs>();
            var c3 = scope3.GetComponent<ScopedComponentWithPlugs>();
            
            Assert.IsNotNull(c0);
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.IsNotNull(c0.Scope);
            Assert.IsNotNull(c1.Scope);
            Assert.IsNotNull(c2.Scope);
            Assert.IsNotNull(c3.Scope);
            
            Assert.AreEqual(c0.Scope, _context);
            Assert.AreEqual(c1.Scope, scope1);
            Assert.AreEqual(c2.Scope, scope2);
            Assert.AreEqual(c3.Scope, scope3);
        }

        [TestMethod]
        public void ScopeDisposalInOrder()
        {
            var scope1 = _context.CreateScope();
            var scope2 = scope1.CreateScope();
            var scope3 = scope2.CreateScope();

            var c0 = _context.GetComponent<ScopedComponent>();
            var c1 = scope1.GetComponent<ScopedComponent>();
            var c2 = scope2.GetComponent<ScopedComponent>();
            var c3 = scope3.GetComponent<ScopedComponent>();
            
            Assert.IsNotNull(c0);
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);

            scope3.Dispose();
            
            Assert.IsTrue(c3.Disposed);
            Assert.IsFalse(c2.Disposed);
            Assert.IsFalse(c1.Disposed);
            Assert.IsFalse(c0.Disposed);
            
            scope2.Dispose();
            
            Assert.IsTrue(c3.Disposed);
            Assert.IsTrue(c2.Disposed);
            Assert.IsFalse(c1.Disposed);
            Assert.IsFalse(c0.Disposed);
            
            scope1.Dispose();
            
            Assert.IsTrue(c3.Disposed);
            Assert.IsTrue(c2.Disposed);
            Assert.IsTrue(c1.Disposed);
            Assert.IsFalse(c0.Disposed);

            _context.Dispose();

            Assert.IsTrue(c3.Disposed);
            Assert.IsTrue(c2.Disposed);
            Assert.IsTrue(c1.Disposed);
            Assert.IsTrue(c0.Disposed);
        }

        [TestMethod]
        public void ScopeDisposalOutOfOrder()
        {
            var scope1 = _context.CreateScope();
            var scope2 = scope1.CreateScope();
            var scope3 = scope2.CreateScope();

            var c0 = _context.GetComponent<ScopedComponent>();
            var c1 = scope1.GetComponent<ScopedComponent>();
            var c2 = scope2.GetComponent<ScopedComponent>();
            var c3 = scope3.GetComponent<ScopedComponent>();
            
            Assert.IsNotNull(c0);
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);

            scope1.Dispose();
            
            Assert.IsTrue(c3.Disposed);
            Assert.IsTrue(c2.Disposed);
            Assert.IsTrue(c1.Disposed);
            Assert.IsFalse(c0.Disposed);

            _context.Dispose();

            Assert.IsTrue(c3.Disposed);
            Assert.IsTrue(c2.Disposed);
            Assert.IsTrue(c1.Disposed);
            Assert.IsTrue(c0.Disposed);
        }

        [TestMethod]
        public void ScopeDisposalByContextRoot()
        {
            var scope1 = _context.CreateScope();
            var scope2 = scope1.CreateScope();
            var scope3 = scope2.CreateScope();

            var c0 = _context.GetComponent<ScopedComponent>();
            var c1 = scope1.GetComponent<ScopedComponent>();
            var c2 = scope2.GetComponent<ScopedComponent>();
            var c3 = scope3.GetComponent<ScopedComponent>();
            
            Assert.IsNotNull(c0);
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);

            Assert.IsFalse(c3.Disposed);
            Assert.IsFalse(c2.Disposed);
            Assert.IsFalse(c1.Disposed);
            Assert.IsFalse(c0.Disposed);

            _context.Dispose();

            Assert.IsTrue(c3.Disposed);
            Assert.IsTrue(c2.Disposed);
            Assert.IsTrue(c1.Disposed);
            Assert.IsTrue(c0.Disposed);
        }
    }
}