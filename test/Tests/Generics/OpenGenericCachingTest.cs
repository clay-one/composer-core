using ComposerCore.Cache;
using ComposerCore.Factories;
using ComposerCore.Implementation;
using ComposerCore.Tests.Generics.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.Generics
{
    [TestClass]
    public class OpenGenericCachingTest
    {
        public TestContext TestContext { get; set; }
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
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void DefaultCacheSameContract()
        {
            _context.Register(typeof(OpenComponentOne<>));
            
            var c1 = _context.GetComponent<IGenericContractOne<string>>();
            var c2 = _context.GetComponent<IGenericContractOne<string>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreSame(c1, c2);
        }

        [TestMethod]
        public void DefaultCacheSameContractMultipleTimes()
        {
            _context.Register(typeof(OpenComponentOne<>));
            
            var c11 = _context.GetComponent<IGenericContractOne<string>>();
            var c12 = _context.GetComponent<IGenericContractOne<object>>();
            
            var c21 = _context.GetComponent<IGenericContractOne<string>>();
            var c22 = _context.GetComponent<IGenericContractOne<object>>();
            
            Assert.IsNotNull(c11);
            Assert.IsNotNull(c12);
            Assert.IsNotNull(c21);
            Assert.IsNotNull(c22);
            Assert.AreSame(c11, c21);
            Assert.AreSame(c12, c22);
            Assert.AreNotSame(c11, c12);
            Assert.AreNotSame(c21, c22);
        }

        [TestMethod]
        public void DefaultCacheDifferentContract()
        {
            _context.Register(typeof(MultiContractOpenComponent<>));

            var c1 = _context.GetComponent<IGenericContractOne<string>>();
            var c2 = _context.GetComponent<IGenericContractTwo<string, string>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreNotSame(c1, c2);
        }

        [TestMethod]
        public void DefaultCacheDifferentContractMultipleTimes()
        {
            _context.Register(typeof(MultiContractOpenComponent<>));

            var c11 = _context.GetComponent<IGenericContractOne<string>>();
            var c12 = _context.GetComponent<IGenericContractTwo<string, string>>();
            
            var c21 = _context.GetComponent<IGenericContractOne<string>>();
            var c22 = _context.GetComponent<IGenericContractTwo<string, string>>();
            
            Assert.IsNotNull(c11);
            Assert.IsNotNull(c12);
            Assert.IsNotNull(c21);
            Assert.IsNotNull(c22);
            Assert.AreSame(c11, c21);
            Assert.AreSame(c12, c22);
            Assert.AreNotSame(c11, c12);
            Assert.AreNotSame(c21, c22);
        }

        [TestMethod]
        public void DefaultCacheDifferentTypeParams()
        {
            _context.Register(typeof(OpenComponentOne<>));
            
            var c1 = _context.GetComponent<IGenericContractOne<string>>();
            var c2 = _context.GetComponent<IGenericContractOne<object>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreNotSame(c1, c2);
        }

        [TestMethod]
        public void Transient()
        {
            _context.Register(typeof(OpenTransientComponent<>));

            var c1 = _context.GetComponent<IGenericContractOne<string>>();
            var c2 = _context.GetComponent<IGenericContractOne<string>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreNotSame(c1, c2);
        }

        [TestMethod]
        public void SingletonSameContract()
        {
            _context.Register(typeof(OpenSingletonComponent<>));

            var c1 = _context.GetComponent<IGenericContractOne<string>>();
            var c2 = _context.GetComponent<IGenericContractOne<string>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreSame(c1, c2);
        }

        [TestMethod]
        public void SingletonSameContractDifferentTypeParams()
        {
            _context.Register(typeof(OpenSingletonComponent<>));

            var c1 = _context.GetComponent<IGenericContractOne<string>>();
            var c2 = _context.GetComponent<IGenericContractOne<object>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreNotSame(c1, c2);
        }

        [TestMethod]
        public void SingletonSameContractDifferentTypeParamsMultipleTimes()
        {
            _context.Register(typeof(OpenSingletonComponent<>));

            var c11 = _context.GetComponent<IGenericContractOne<string>>();
            var c12 = _context.GetComponent<IGenericContractOne<object>>();
            
            var c21 = _context.GetComponent<IGenericContractOne<string>>();
            var c22 = _context.GetComponent<IGenericContractOne<object>>();
            
            Assert.IsNotNull(c11);
            Assert.IsNotNull(c12);
            Assert.IsNotNull(c21);
            Assert.IsNotNull(c22);
            Assert.AreSame(c11, c21);
            Assert.AreSame(c12, c22);
            Assert.AreNotSame(c11, 12);
            Assert.AreNotSame(c21, 22);
        }

        [TestMethod]
        public void SingletonDifferentContracts()
        {
            _context.Register(new ComponentRegistration(
                new GenericLocalComponentFactory(typeof(MultiContractOpenComponent<>)), 
                nameof(SingletonComponentCache)));

            var c1 = _context.GetComponent<IGenericContractOne<string>>();
            var c2 = _context.GetComponent<IGenericContractTwo<string, string>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreSame(c1, c2);
        }
    }
}