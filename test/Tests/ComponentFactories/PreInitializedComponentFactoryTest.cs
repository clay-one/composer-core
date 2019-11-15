using ComposerCore.Factories;
using ComposerCore.Implementation;
using ComposerCore.Tests.ComponentFactories.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#pragma warning disable 612

namespace ComposerCore.Tests.ComponentFactories
{
    [TestClass]
    public class PreInitializedComponentFactoryTest
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
        public void RegisterAndQuery()
        {
            var component = new SampleComponentAndContract();
            var factory = new PreInitializedComponentFactory(component);
            
            _context.Register(factory);
            
            Assert.IsTrue(_context.IsResolvable<SampleComponentAndContract>());

            var c = _context.GetComponent<SampleComponentAndContract>();
            Assert.IsNotNull(c);
            Assert.AreSame(component, c);
        }

        [TestMethod]
        public void RegisterUnattributedWithoutContract()
        {
            var component = new UnattributedComponent();
            var factory = new PreInitializedComponentFactory(component);
            
            Expect.ToThrow<CompositionException>(() => _context.Register(factory));
        }

        [TestMethod]
        public void RegisterUnattributedWithSelf()
        {
            var component = new UnattributedComponent();
            var factory = new PreInitializedComponentFactory(component);

            _context.Register(typeof(UnattributedComponent), factory);

            Assert.IsTrue(_context.IsResolvable<UnattributedComponent>());
            Assert.IsFalse(_context.IsResolvable<IUnattributedContract>());
            
            var c = _context.GetComponent<UnattributedComponent>();
            Assert.IsNotNull(c);
            Assert.AreSame(component, c);
        }
        
        [TestMethod]
        public void RegisterUnattributedWithInterface()
        {
            var component = new UnattributedComponent();
            var factory = new PreInitializedComponentFactory(component);

            _context.Register(typeof(IUnattributedContract), factory);

            Assert.IsFalse(_context.IsResolvable<UnattributedComponent>());
            Assert.IsTrue(_context.IsResolvable<IUnattributedContract>());
            
            var c = _context.GetComponent<IUnattributedContract>();
            Assert.IsNotNull(c);
            Assert.AreSame(component, c);
        }

        [TestMethod]
        public void RegisterWithInvalidContract()
        {
            var component = new SampleComponentAndContract();
            var factory = new PreInitializedComponentFactory(component);

            Expect.ToThrow<CompositionException>(() => _context.Register(typeof(IUnattributedContract), factory));
        }
        
    }
}