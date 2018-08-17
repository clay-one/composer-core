using ComposerCore.Implementation;
using ComposerCore.Tests.ComponentCaching.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ComponentCaching
{
    [TestClass]
    public class ThreadLocalTest
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
            _context.Register(typeof(ThreadLocalComponent));
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void TestGetThreadLocalComponent()
        {
            var component = _context.GetComponent<ISomeContract>();
            Assert.IsNotNull(component);
        }

    }
}