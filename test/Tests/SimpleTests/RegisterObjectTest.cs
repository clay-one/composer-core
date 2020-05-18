using ComposerCore.Implementation;
using ComposerCore.Tests.SimpleTests.SimpleTestComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.SimpleTests
{
    [TestClass]
    public class RegisterObjectTest
    {
        private ComponentContext _context;

        #region Initialization and Cleanup methods

        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
        }

        [TestInitialize]
        public void Initialize()
        {
            _context = new ComponentContext();
        }

        [TestCleanup]
        public void Cleanup()
        {
        }

        #endregion

        [TestMethod]
        public void RegisterWithoutContractAndName()
        {
            var componentInstance = new EmptyComponentAndContract();
            _context.RegisterObject(componentInstance);

            var c = _context.GetComponent<EmptyComponentAndContract>();
            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void ReturnsTheSameObject()
        {
            var componentInstance = new EmptyComponentAndContract();
            
            _context.RegisterObject(componentInstance);
            var c1 = _context.GetComponent<EmptyComponentAndContract>();
            var c2 = _context.GetComponent<EmptyComponentAndContract>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.AreSame(c1, componentInstance);
            Assert.AreSame(c2, componentInstance);
        }

        [TestMethod]
        public void RegisterWithName()
        {
            var componentInstance = new EmptyComponentAndContract();
            _context.RegisterObject("someName", componentInstance);

            var c1 = _context.GetComponent<EmptyComponentAndContract>("someName");
            var c2 = _context.GetComponent<EmptyComponentAndContract>("anotherName");
            var c3 = _context.GetComponent<EmptyComponentAndContract>();
            
            Assert.IsNotNull(c1);
            Assert.IsNull(c2);
            Assert.IsNull(c3);
        }

        [TestMethod]
        public void RegisterWithContract()
        {
            var componentInstance = new EmptyComponent();
            _context.RegisterObject(typeof(IEmptyContract), componentInstance);

            var c1 = _context.GetComponent<IEmptyContract>();
            var c2 = _context.GetComponent<EmptyComponent>();
            
            Assert.IsNotNull(c1);
            Assert.IsNull(c2);
        }

        [TestMethod]
        public void RegisterWithInvalidContract()
        {
            var componentInstance = new EmptyComponentAndContract();
            Expect.ToThrow<CompositionException>(() =>
                _context.RegisterObject(typeof(IEmptyContract), componentInstance));
        }

        [TestMethod]
        public void RegisterComponentThatHasMultipleContracts()
        {
            var componentInstance = new EmptyComponentWithMultipleContracts();
            _context.RegisterObject(componentInstance);

            var c1 = _context.GetComponent<EmptyComponentWithMultipleContracts>();
            var c2 = _context.GetComponent<EmptyComponentAndContract>();
            var c3 = _context.GetComponent<IEmptyContract>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
        }

        [TestMethod]
        public void RegisterComponentThatHasMultipleContractsWithSingleContract()
        {
            var componentInstance = new EmptyComponentWithMultipleContracts();
            _context.RegisterObject(typeof(IEmptyContract), componentInstance);

            var c1 = _context.GetComponent<EmptyComponentWithMultipleContracts>();
            var c2 = _context.GetComponent<EmptyComponentAndContract>();
            var c3 = _context.GetComponent<IEmptyContract>();
            
            Assert.IsNull(c1);
            Assert.IsNull(c2);
            Assert.IsNotNull(c3);
        }

        [TestMethod]
        public void RegisterWithContractAndName()
        {
            var componentInstance = new EmptyComponent();
            _context.RegisterObject(typeof(IEmptyContract), "someName", componentInstance);

            var c1 = _context.GetComponent<IEmptyContract>();
            var c2 = _context.GetComponent<IEmptyContract>("someName");
            var c3 = _context.GetComponent<EmptyComponent>();
            
            Assert.IsNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNull(c3);
        }
    }
}