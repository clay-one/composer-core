using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Tests.FluentRegistration.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.FluentRegistration
{
    [TestClass]
    public class FluentBasicRegistrationTest
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
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void SimpleRegister()
        {
            _context.ForComponent<NonAttributedComponent>().RegisterWith<INonAttributedContract>();

            var c = _context.GetComponent<INonAttributedContract>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void SimpleRegisterWithoutExplicitContract()
        {
            _context.ForComponent<ComponentOne>().Register();

            var c = _context.GetComponent<IComponentOne>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void SimpleRegisterWithType()
        {
            _context.ForComponent(typeof(NonAttributedComponent)).RegisterWith(typeof(INonAttributedContract));

            var c = _context.GetComponent<INonAttributedContract>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void NamedRegister()
        {
            _context.ForComponent<NonAttributedComponent>().RegisterWith<INonAttributedContract>("name");

            var c = _context.GetComponent<INonAttributedContract>("name");
            Assert.IsNotNull(c);

            var wrongC = _context.GetComponent<INonAttributedContract>();
            Assert.IsNull(wrongC);
        }

        [TestMethod]
        public void RegisterWithSelfType()
        {
            _context.ForComponent<NonAttributedComponent>()
                .RegisterWith<NonAttributedComponent>();

            var c = _context.GetComponent<NonAttributedComponent>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void RegisterAsItself()
        {
            _context.ForComponent<NonAttributedComponent>().RegisterAsItself();

            var c = _context.GetComponent<NonAttributedComponent>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void RegisterObjectWithContract()
        {
            var o = new NonAttributedComponent();

#pragma warning disable 618
            _context.ForObject(o).RegisterWith<INonAttributedContract>();
#pragma warning restore 618

            var c = _context.GetComponent<INonAttributedContract>();

            Assert.IsNotNull(c);
            Assert.IsTrue(ReferenceEquals(c, o));
        }

        [TestMethod]
        public void RegisterObjectWithoutExplicitContract()
        {
            var o = new ComponentOne();

#pragma warning disable 618
            _context.ForObject(o).Register();
#pragma warning restore 618

            var c = _context.GetComponent<IComponentOne>();

            Assert.IsNotNull(c);
            Assert.IsTrue(ReferenceEquals(c, o));
        }

        [TestMethod]
        public void RegisterWithInvalidContract()
        {
            Expect.ToThrow<CompositionException>(() => _context.ForComponent<ComponentOne>().RegisterWith<IComponentTwo>());
        }
    }
}