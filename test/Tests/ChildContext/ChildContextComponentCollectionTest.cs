using System.Linq;
using ComposerCore.Implementation;
using ComposerCore.Tests.ChildContext.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable PossibleMultipleEnumeration

namespace ComposerCore.Tests.ChildContext
{
    [TestClass]
    public class ChildContextComponentCollectionTest
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
        public void ParentRegistrationAppearsInChildQuery()
        {
            _context.Register<ComponentOneA>();

            var e = _childContext.GetAllComponents<IContractOne>();
            
            Assert.IsNotNull(e);
            Assert.IsTrue(e.Any());
            Assert.IsTrue(e.Count() == 1);

            var c = e.Single();
            Assert.IsNotNull(c);
            Assert.IsInstanceOfType(c, typeof(ComponentOneA));
        }

        [TestMethod]
        public void ChildRegistrationDoesNotAppearInParentQuery()
        {
            _childContext.Register<ComponentOneA>();

            var e = _context.GetAllComponents<IContractOne>();
            
            Assert.IsNotNull(e);
            Assert.IsFalse(e.Any());
        }

        [TestMethod]
        public void DifferentRegistrationsInParentAndChild()
        {
            _context.Register<ComponentOneA>();
            _childContext.Register<ComponentOneB>();

            var e1 = _context.GetAllComponents<IContractOne>();
            
            Assert.IsNotNull(e1);
            Assert.IsTrue(e1.Any());
            Assert.AreEqual(e1.Count(), 1);
            Assert.IsNotNull(e1.Single());
            Assert.IsInstanceOfType(e1.Single(), typeof(ComponentOneA));
            
            var e2 = _childContext.GetAllComponents<IContractOne>();
            
            Assert.IsNotNull(e2);
            Assert.IsTrue(e2.Any());
            Assert.AreEqual(e2.Count(), 2);
            Assert.IsTrue(e2.All(e => e != null));
            Assert.IsTrue(e2.Any(e => e is ComponentOneA));
            Assert.IsTrue(e2.Any(e => e is ComponentOneB));
        }

        [TestMethod]
        public void RedundantRegistrationsInParentAndChild()
        {
            _context.Register<ComponentOneA>();
            _context.Register<ComponentOneB>();
            _childContext.Register<ComponentOneA>();
            _childContext.Register<ComponentOneB>();

            var e1 = _context.GetAllComponents<IContractOne>();
            
            Assert.IsNotNull(e1);
            Assert.IsTrue(e1.Any());
            Assert.IsTrue(e1.Count() == 2);
            Assert.IsTrue(e1.All(e => e != null));
            Assert.AreEqual(e1.Count(e => e is ComponentOneA), 1);
            Assert.AreEqual(e1.Count(e => e is ComponentOneB), 1);
            
            var e2 = _childContext.GetAllComponents<IContractOne>();
            
            Assert.IsNotNull(e2);
            Assert.IsTrue(e2.Any());
            Assert.AreEqual(e2.Count(), 4);
            Assert.IsTrue(e2.All(e => e != null));
            Assert.AreEqual(e2.Count(e => e is ComponentOneA), 2);
            Assert.AreEqual(e2.Count(e => e is ComponentOneB), 2);
        }
    }
}