using System.Linq;
using ComposerCore.Attributes;
using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Tests.FluentRegistration.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.FluentRegistration
{
    [TestClass]
    public class FluentConstructorCompositionTest
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
            _context.Configuration.DisableAttributeChecking = true;
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostResolvable;

            _context.Register(typeof(ComponentOne));
            _context.Register(typeof(IComponentTwo), "name", typeof(ComponentTwo));
            _context.SetVariableValue("one", "someString");
            _context.SetVariableValue("two", 5);
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void ConstructorComponent()
        {
            _context.ForComponent<NonAttributedComponent>()
                .AddConstructorComponent<IComponentOne>()
                .AddConstructorComponent<IComponentTwo>("name")
                .RegisterWith<INonAttributedContract>();

            var x = _context.GetComponent<IComponentOne>();
            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNotNull(c.ComponentTwo);
        }

        [TestMethod]
        public void OptionalConstructorComponent()
        {
            _context.ForComponent<NonAttributedComponent>()
                .UseConstructor(typeof(IComponentOne), typeof(IComponentTwo))
                .AddConstructorComponent<IComponentOne>(required: false)
                .AddConstructorComponent<IComponentTwo>(required: false)
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNull(c.ComponentTwo);
        }

        [TestMethod]
        public void ConstructorComponentAsValue()
        {
            _context.ForComponent<NonAttributedComponent>()
                .AddConstructorValue(new ComponentOne())
                .AddConstructorValue(new ComponentTwo())
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNotNull(c.ComponentTwo);
        }

        [TestMethod]
        public void ConstructorValue()
        {
            _context.ForComponent<NonAttributedComponent>()
                .AddConstructorValue("someString")
                .AddConstructorValue(5)
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.AreEqual("someString", c.SomeValue);
            Assert.AreEqual(5, c.SomeOtherValue);
        }

        [TestMethod]
        public void ConstructorValueDelegate()
        {
            _context.ForComponent<NonAttributedComponent>()
                .AddConstructorValue(cmpsr => cmpsr.GetComponent<IComponentOne>())
                .AddConstructorValue(cmpsr => cmpsr.GetComponent<IComponentTwo>("name"))
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNotNull(c.ComponentTwo);
        }

        [TestMethod]
        public void ConstructorVariable()
        {
            _context.ForComponent<NonAttributedComponent>()
                .AddConstructorValueFromVariable("one")
                .AddConstructorValueFromVariable("two")
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.AreEqual("someString", c.SomeValue);
            Assert.AreEqual(5, c.SomeOtherValue);
        }

        [TestMethod]
        public void MixedConstructorArgs()
        {
            _context.ForComponent<NonAttributedComponent>()
                .AddConstructorValue(cmpsr => new ComponentOne())
                .AddConstructorComponent<IComponentTwo>("name")
                .AddConstructorValueFromVariable("one")
                .AddConstructorValue(5)
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNotNull(c.ComponentTwo);
            Assert.AreEqual("someString", c.SomeValue);
            Assert.AreEqual(5, c.SomeOtherValue);
        }

        [TestMethod]
        public void SetConstructorResolutionPolicy()
        {
            _context.ForComponent<NonAttributedComponent>()
                .SetConstructorResolutionPolicy(ConstructorResolutionPolicy.MostResolvable)
                .RegisterWith<INonAttributedContract>();
            
            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNull(c.ComponentTwo);
            Assert.IsNull(c.SomeValue);
            Assert.AreEqual(default, c.SomeOtherValue);
        }

        [TestMethod]
        public void SetConstructorResolutionWithGetAllComponents()
        {
            _context.ForComponent<NonAttributedComponent>()
                .SetConstructorResolutionPolicy(ConstructorResolutionPolicy.MostResolvable)
                .RegisterWith<INonAttributedContract>();
            
            _context.ForComponent<NonAttributedComponent>()
                .SetConstructorResolutionPolicy(ConstructorResolutionPolicy.DefaultConstructor)
                .RegisterWith<INonAttributedContract>();

            var ii = _context.GetAllComponents<INonAttributedContract>().ToList();
            
            Assert.IsNotNull(ii);
            Assert.IsTrue(ii.All(i => i is NonAttributedComponent));

            var cc = ii.Cast<NonAttributedComponent>().ToList();
            
            Assert.IsNotNull(cc);
            Assert.IsTrue(cc.All(c => c != null));
            Assert.IsTrue(cc.All(c => c.ComponentTwo == default));
            Assert.IsTrue(cc.All(c => c.SomeValue == default));
            Assert.IsTrue(cc.All(c => c.SomeOtherValue == default));
            
            Assert.IsTrue(cc.Any(c => c.ComponentOne == default));
            Assert.IsTrue(cc.Any(c => c.ComponentOne != default));
        }
    }
}