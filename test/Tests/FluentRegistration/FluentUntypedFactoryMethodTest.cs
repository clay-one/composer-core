using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Tests.FluentRegistration.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.FluentRegistration
{
    [TestClass]
    public class FluentUntypedFactoryMethodTest
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
        public void RegistrationWithSpecificContract()
        {
            _context.ForUntypedFactoryMethod(composer => new ComponentWithManyContracts())
                .RegisterWith<IComponentOne>();
            
            Assert.IsTrue(_context.IsResolvable<IComponentOne>());
            Assert.IsFalse(_context.IsResolvable<IComponentTwo>());
            Assert.IsNotNull(_context.GetComponent<IComponentOne>());
            Assert.IsNull(_context.GetComponent<IComponentTwo>());
        }

        [TestMethod]
        public void TransientRegistration()
        {
            _context.ForUntypedFactoryMethod(composer => new ComponentWithManyContracts())
                .AsTransient()
                .RegisterWith<IComponentOne>();

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
            _context.ForUntypedFactoryMethod(composer => new ComponentWithManyContracts())
                .AsSingleton()
                .RegisterWith<IComponentOne>();

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
            _context.ForUntypedFactoryMethod(composer => new SingletonComponent())
                .AsTransient()
                .RegisterWith<IComponentOne>();

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