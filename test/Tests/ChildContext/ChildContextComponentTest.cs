using ComposerCore.Implementation;
using ComposerCore.Tests.ChildContext.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ChildContext
{
    [TestClass]
    public class ChildContextComponentTest
    {
        private ComponentContext _context;
        private ChildComponentContext _childContext;

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
            _childContext = new ChildComponentContext(_context);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void ParentRegistrationQueriedFromChild()
        {
            _context.Register<ComponentOneA>();

            Assert.IsTrue(_context.IsResolvable<IContractOne>());
            Assert.IsTrue(_childContext.IsResolvable<IContractOne>());
            
            var c1 = _context.GetComponent<IContractOne>();
            var c2 = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsTrue(c1 is ComponentOneA);
            Assert.IsTrue(c2 is ComponentOneA);
        }

        [TestMethod]
        public void ChildRegistrationQueriedFromParent()
        {
            _childContext.Register<ComponentOneA>();

            Assert.IsFalse(_context.IsResolvable<IContractOne>());
            Assert.IsTrue(_childContext.IsResolvable<IContractOne>());

            var c1 = _context.GetComponent<IContractOne>();
            var c2 = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNull(c1);
            Assert.IsNotNull(c2);
        }

        [TestMethod]
        public void DifferentRegistrationInParentAndChild()
        {
            _context.Register<ComponentOneA>();
            _childContext.Register<ComponentOneB>();

            var ca = _context.GetComponent<IContractOne>();
            var cb = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            
            Assert.IsTrue(ca is ComponentOneA);
            Assert.IsTrue(cb is ComponentOneB);
        }

        [TestMethod]
        public void ChildRegistrationOverridesParent()
        {
            _context.Register<ComponentOneA>();

            var ca = _context.GetComponent<IContractOne>();
            var cb = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            
            Assert.IsTrue(ca is ComponentOneA);
            Assert.IsTrue(cb is ComponentOneA);
            
            _childContext.Register<ComponentOneB>();
            
            ca = _context.GetComponent<IContractOne>();
            cb = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            
            Assert.IsTrue(ca is ComponentOneA);
            Assert.IsTrue(cb is ComponentOneB);
        }

        [TestMethod]
        public void ChildUnregisterRevertsToParent()
        {
            _context.Register<ComponentOneA>();
            _childContext.Register<ComponentOneB>();
            
            var ca = _context.GetComponent<IContractOne>();
            var cb = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            
            Assert.IsTrue(ca is ComponentOneA);
            Assert.IsTrue(cb is ComponentOneB);
            
            _childContext.Unregister(typeof(IContractOne));
            
            ca = _context.GetComponent<IContractOne>();
            cb = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            
            Assert.IsTrue(ca is ComponentOneA);
            Assert.IsTrue(cb is ComponentOneA);
        }

        [TestMethod]
        public void ChildUnregisterFamilyRevertsToParent()
        {
            _context.Register<ComponentOneA>();
            _childContext.Register<ComponentOneB>();
            
            var ca = _context.GetComponent<IContractOne>();
            var cb = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            
            Assert.IsTrue(ca is ComponentOneA);
            Assert.IsTrue(cb is ComponentOneB);
            
            _childContext.UnregisterFamily(typeof(IContractOne));
            
            ca = _context.GetComponent<IContractOne>();
            cb = _childContext.GetComponent<IContractOne>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            
            Assert.IsTrue(ca is ComponentOneA);
            Assert.IsTrue(cb is ComponentOneA);
        }

        [TestMethod]
        public void SingletonQueriedFromChildIsTheSame()
        {
            _context.Register<ComponentOneSingleton>();
            
            var ca = _context.GetComponent<IContractOne>();
            var cb = _childContext.GetComponent<IContractOne>();

            Assert.IsNotNull(ca);
            Assert.IsNotNull(cb);
            Assert.AreSame(ca, cb);
        }

        [TestMethod]
        public void ParentRegistrationQueriedFromParentPlug()
        {
            _context.Register<ComponentOneA>();
            _context.Register<ComponentTwoA>();
            _context.Register<ComponentWithPlugs>();

            var c = _childContext.GetComponent<ComponentWithPlugs>();
            
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.One);
            Assert.IsNotNull(c.Two);
        }
        
        [TestMethod]
        public void ParentRegistrationQueriedFromChildPlug()
        {
            _context.Register<ComponentOneA>();
            _context.Register<ComponentTwoA>();
            _childContext.Register<ComponentWithPlugs>();

            var c = _childContext.GetComponent<ComponentWithPlugs>();
            
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.One);
            Assert.IsNotNull(c.Two);
        }

        [TestMethod]
        public void ChildPlugRegistrationAffectsChildResolution()
        {
            _context.Register<ComponentOneA>();
            _context.Register<ComponentTwoA>();
            _context.Register<ComponentWithPlugs>();
            
            _childContext.Register<ComponentOneB>();
            _childContext.Register<ComponentTwoB>();
            _childContext.Register<ComponentWithPlugs>();

            var ca = _context.GetComponent<ComponentWithPlugs>();
            var cb = _childContext.GetComponent<ComponentWithPlugs>();
            
            Assert.IsNotNull(ca);
            Assert.IsNotNull(ca.One);
            Assert.IsNotNull(ca.Two);
            Assert.IsNotNull(cb);
            Assert.IsNotNull(cb.One);
            Assert.IsNotNull(cb.Two);
            
            Assert.IsTrue(ca.One is ComponentOneA);
            Assert.IsTrue(ca.Two is ComponentTwoA);
            
            Assert.IsTrue(cb.One is ComponentOneB);
            Assert.IsTrue(cb.Two is ComponentTwoB);
        }

        [TestMethod]
        public void ChildPlugRegistrationDoesNotAffectParentResolution()
        {
            _context.Register<ComponentOneA>();
            _context.Register<ComponentTwoA>();
            _context.Register<ComponentWithPlugs>();
            
            _childContext.Register<ComponentOneB>();
            _childContext.Register<ComponentTwoB>();

            var c = _childContext.GetComponent<ComponentWithPlugs>();
            
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.One);
            Assert.IsNotNull(c.Two);
            
            Assert.IsTrue(c.One is ComponentOneA);
            Assert.IsTrue(c.Two is ComponentTwoA);
        }
    }
}