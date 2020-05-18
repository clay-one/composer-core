using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Tests.FluentRegistration.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.FluentRegistration
{
    [TestClass]
    public class FluentFactoryMethodTest
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
            _context.Configuration.DisableAttributeChecking = true;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void RegistrationAndLookup()
        {
            _context.ForFactoryMethod(composer => new ComponentWithManyContracts()).Register();
            
            Assert.IsTrue(_context.IsResolvable<IComponentOne>());
            Assert.IsTrue(_context.IsResolvable<IComponentTwo>());
            Assert.IsNotNull(_context.GetComponent<IComponentOne>());
            Assert.IsNotNull(_context.GetComponent<IComponentTwo>());
        }

        [TestMethod]
        public void RegistrationWithSpecificContract()
        {
            _context.ForFactoryMethod(composer => new ComponentWithManyContracts())
                .RegisterWith<IComponentOne>();
            
            Assert.IsTrue(_context.IsResolvable<IComponentOne>());
            Assert.IsFalse(_context.IsResolvable<IComponentTwo>());
            Assert.IsNotNull(_context.GetComponent<IComponentOne>());
            Assert.IsNull(_context.GetComponent<IComponentTwo>());
        }

        [TestMethod]
        public void TransientRegistration()
        {
            _context.ForFactoryMethod(composer => new ComponentWithManyContracts())
                .AsTransient()
                .Register();

            var c1 = _context.GetComponent<IComponentOne>();
            var c2 = _context.GetComponent<IComponentOne>();
            var c3 = _context.GetComponent<IComponentOne>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c2, c3);
            Assert.AreNotSame(c1, c3);
        }

        [TestMethod]
        public void SingletonRegistration()
        {
            _context.ForFactoryMethod(composer => new ComponentWithManyContracts())
                .AsSingleton()
                .Register();

            var c1 = _context.GetComponent<IComponentOne>();
            var c2 = _context.GetComponent<IComponentOne>();
            var c3 = _context.GetComponent<IComponentOne>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreSame(c1, c2);
            Assert.AreSame(c2, c3);
            Assert.AreSame(c1, c3);
        }

        [TestMethod]
        public void TransientComponent()
        {
            _context.ForFactoryMethod(composer => new TransientComponent()).Register();

            var c1 = _context.GetComponent<IComponentOne>();
            var c2 = _context.GetComponent<IComponentOne>();
            var c3 = _context.GetComponent<IComponentOne>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c2, c3);
            Assert.AreNotSame(c1, c3);
        }

        [TestMethod]
        public void SingletonComponent()
        {
            _context.ForFactoryMethod(composer => new SingletonComponent()).Register();

            var c1 = _context.GetComponent<IComponentOne>();
            var c2 = _context.GetComponent<IComponentOne>();
            var c3 = _context.GetComponent<IComponentOne>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreSame(c1, c2);
            Assert.AreSame(c2, c3);
            Assert.AreSame(c1, c3);
        }

        [TestMethod]
        public void RegistrationCacheTypeOverridesAttribute()
        {
            _context.ForFactoryMethod(composer => new SingletonComponent()).AsTransient().Register();

            var c1 = _context.GetComponent<IComponentOne>();
            var c2 = _context.GetComponent<IComponentOne>();
            var c3 = _context.GetComponent<IComponentOne>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c2, c3);
            Assert.AreNotSame(c1, c3);
        }
    }
}