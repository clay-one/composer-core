using ComposerCore.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ChildContext
{
    [TestClass]
    public class ChildContextVariableTest
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
        public void QueryParentVariableUsingChild()
        {
            var value = new object();
            
            _context.SetVariableValue("name", value);
            var v = _childContext.GetVariable("name");
            
            Assert.IsNotNull(v);
            Assert.AreSame(value, v);
        }

        [TestMethod]
        public void ChildValueOverridesParent()
        {
            var value1 = new object();
            var value2 = new object();
            
            _context.SetVariableValue("name", value1);
            _childContext.SetVariableValue("name", value2);
            
            var v = _childContext.GetVariable("name");
            
            Assert.IsNotNull(v);
            Assert.AreNotSame(value1, v);
            Assert.AreSame(value2, v);
        }

        [TestMethod]
        public void RemoveVariableInChild()
        {
            var value1 = new object();
            var value2 = new object();
            
            _context.SetVariableValue("name", value1);
            _childContext.SetVariableValue("name", value2);
            
            Assert.AreSame(value2, _childContext.GetVariable("name"));

            _childContext.RemoveVariable("name");
            
            Assert.AreSame(value1, _childContext.GetVariable("name"));
        }
    }
}