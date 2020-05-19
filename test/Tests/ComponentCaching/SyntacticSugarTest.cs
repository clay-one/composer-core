using ComposerCore.Implementation;
using ComposerCore.Tests.ComponentCaching.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ComponentCaching
{
    [TestClass]
    public class SyntacticSugarTest
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
            _context.Register(typeof(SingletonComponent));
            _context.Register(typeof(TransientComponent));
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void CheckTransientComponent()
        {
            var c1 = _context.GetComponent<TransientComponent>();
            var c2 = _context.GetComponent<TransientComponent>();
            var c3 = _context.GetComponent<ISomeContract>();
            var c4 = _context.GetComponent<ISomeContract>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            Assert.IsNotNull(c4);

            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c1, c3);
            Assert.AreNotSame(c1, c4);
            Assert.AreNotSame(c2, c3);
            Assert.AreNotSame(c2, c4);
            Assert.AreNotSame(c3, c4);
        }

        [TestMethod]
        public void CheckSingletonComponent()
        {
            var c1 = _context.GetComponent<SingletonComponent>();
            var c2 = _context.GetComponent<SingletonComponent>();
            var c3 = _context.GetComponent<IAnotherContract>();
            var c4 = _context.GetComponent<IAnotherContract>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            Assert.IsNotNull(c4);

            Assert.AreSame(c1, c2);
            Assert.AreSame(c1, c3);
            Assert.AreSame(c1, c4);
        }
    }
}