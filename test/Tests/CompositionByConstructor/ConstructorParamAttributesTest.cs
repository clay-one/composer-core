using ComposerCore.Implementation;
using ComposerCore.Tests.CompositionByConstructor.AttrComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.CompositionByConstructor
{
    [TestClass]
    public class ConstructorParamAttributesTest
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
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void RequiredPlug()
        {
            _context = new ComponentContext();
            _context.Register(typeof(RequiredPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            Expect.ToThrow<CompositionException>(() => _context.GetComponent<RequiredPlugComponent>());

            _context = new ComponentContext();
            _context.Register(typeof(RequiredPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            Expect.ToThrow<CompositionException>(() => _context.GetComponent<RequiredPlugComponent>());

            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            var c = _context.GetComponent<RequiredPlugComponent>();
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.A);
            Assert.IsNotNull(c.B);
        }

        [TestMethod]
        public void OptionalPlug()
        {
            _context = new ComponentContext();
            _context.Register(typeof(OptionalPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c1 = _context.GetComponent<OptionalPlugComponent>();
            Assert.IsNotNull(c1);
            Assert.IsNull(c1.A);
            Assert.IsNull(c1.B);

            _context = new ComponentContext();
            _context.Register(typeof(OptionalPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            var c2 = _context.GetComponent<OptionalPlugComponent>();
            Assert.IsNotNull(c2);
            Assert.IsNull(c2.A);
            Assert.IsNull(c2.B);

            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            var c3 = _context.GetComponent<OptionalPlugComponent>();
            Assert.IsNotNull(c3);
            Assert.IsNotNull(c3.A);
            Assert.IsNotNull(c3.B);
        }

        [TestMethod]
        public void UnspecifiedPlug()
        {
            _context = new ComponentContext();
            _context.Register(typeof(UnspecifiedPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            Expect.ToThrow<CompositionException>(() => _context.GetComponent<UnspecifiedPlugComponent>());

            _context = new ComponentContext();
            _context.Register(typeof(UnspecifiedPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c1 = _context.GetComponent<UnspecifiedPlugComponent>();
            Assert.IsNotNull(c1);
            Assert.IsNull(c1.A);
            Assert.IsNull(c1.B);

            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            var c2 = _context.GetComponent<UnspecifiedPlugComponent>();
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c2.A);
            Assert.IsNotNull(c2.B);
        }

        [TestMethod]
        public void MixedRequiredAndOptionalAndUnspecifiedPlugd()
        {
            _context = new ComponentContext();
            _context.Register(typeof(MixedRequiredOptionalUnspecifiedPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            Expect.ToThrow<CompositionException>(() =>
                _context.GetComponent<MixedRequiredOptionalUnspecifiedPlugComponent>());

            _context = new ComponentContext();
            _context.Register(typeof(MixedRequiredOptionalUnspecifiedPlugComponent));
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            Expect.ToThrow<CompositionException>(() =>
                _context.GetComponent<MixedRequiredOptionalUnspecifiedPlugComponent>());

            _context = new ComponentContext();
            _context.Register(typeof(MixedRequiredOptionalUnspecifiedPlugComponent));
            _context.Register(typeof(SampleComponentD));
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            Expect.ToThrow<CompositionException>(() =>
                _context.GetComponent<MixedRequiredOptionalUnspecifiedPlugComponent>());

            _context = new ComponentContext();
            _context.Register(typeof(MixedRequiredOptionalUnspecifiedPlugComponent));
            _context.Register(typeof(SampleComponentD));
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c1 = _context.GetComponent<MixedRequiredOptionalUnspecifiedPlugComponent>();
            Assert.IsNotNull(c1);
            Assert.IsNull(c1.A);
            Assert.IsNull(c1.B);
            Assert.IsNull(c1.C);
            Assert.IsNotNull(c1.D);

            _context = new ComponentContext();
            _context.Register(typeof(MixedRequiredOptionalUnspecifiedPlugComponent));
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(SampleComponentC));
            _context.Register(typeof(SampleComponentD));
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c2 = _context.GetComponent<MixedRequiredOptionalUnspecifiedPlugComponent>();
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c2.A);
            Assert.IsNotNull(c2.B);
            Assert.IsNotNull(c2.C);
            Assert.IsNotNull(c2.D);
        }

        [TestMethod]
        public void PlugWithContractName()
        {
            _context = new ComponentContext();
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            _context.Register(typeof(PlugWithContractNameComponent));
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            Expect.ToThrow<CompositionException>(() => _context.GetComponent<PlugWithContractNameComponent>());

            _context = new ComponentContext();
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            _context.Register(typeof(PlugWithContractNameComponent));
            _context.Register("wrongName", typeof(SampleComponentA));
            _context.Register("wrongName", typeof(SampleComponentB));
            Expect.ToThrow<CompositionException>(() => _context.GetComponent<PlugWithContractNameComponent>());

            _context = new ComponentContext();
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            _context.Register(typeof(PlugWithContractNameComponent));
            _context.Register("someName", typeof(SampleComponentA));
            _context.Register("anotherName", typeof(SampleComponentB));
            var c = _context.GetComponent<PlugWithContractNameComponent>();
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.A);
            Assert.IsNotNull(c.B);
        }

        [TestMethod]
        public void ObsoleteMethodAttributeOverridesParamAttributes()
        {
            _context = new ComponentContext();
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            _context.Register(typeof(ObsoleteMethodAttributeOverridesParamAttributeComponent));
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            var c1 = _context.GetComponent<ObsoleteMethodAttributeOverridesParamAttributeComponent>();
            Assert.IsNotNull(c1);
            Assert.IsNull(c1.A);
            Assert.IsNull(c1.B);
            
            _context.Register("ignoredName", typeof(SampleComponentA));
            _context.Register("ignoredName", typeof(SampleComponentB));
            var c2 = _context.GetComponent<ObsoleteMethodAttributeOverridesParamAttributeComponent>();
            Assert.IsNotNull(c2);
            Assert.IsNull(c2.A);
            Assert.IsNull(c2.B);

            _context.Register("someName", typeof(SampleComponentA));
            _context.Register("anotherName", typeof(SampleComponentB));
            var c3 = _context.GetComponent<ObsoleteMethodAttributeOverridesParamAttributeComponent>();
            Assert.IsNotNull(c3);
            Assert.IsNotNull(c3.A);
            Assert.IsNotNull(c3.B);
        }

        [TestMethod]
        public void ConfigWithoutName()
        {
            Assert.Inconclusive("Feature is not yet implemented.");
        }

        [TestMethod]
        public void ConfigWithName()
        {
            Assert.Inconclusive("Feature is not yet implemented.");
        }

        [TestMethod]
        public void RequiredConfig()
        {
            Assert.Inconclusive("Feature is not yet implemented.");
        }

        [TestMethod]
        public void OptionalConfig()
        {
            Assert.Inconclusive("Feature is not yet implemented.");
        }

        [TestMethod]
        public void UnspecifiedConfig()
        {
            Assert.Inconclusive("Feature is not yet implemented.");
        }

        [TestMethod]
        public void ClassQualifiedNameOverrides()
        {
            Assert.Inconclusive("Feature is not yet implemented.");
        }

        [TestMethod]
        public void FullyQualifiedNameOverrides()
        {
            Assert.Inconclusive("Feature is not yet implemented.");
        }
    }
}