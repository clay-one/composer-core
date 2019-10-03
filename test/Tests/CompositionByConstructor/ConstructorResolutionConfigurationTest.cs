using ComposerCore.Attributes;
using ComposerCore.Implementation;
using ComposerCore.Tests.CompositionByConstructor.Components;
using ComposerCore.Tests.CompositionByConstructor.Resolvers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.CompositionByConstructor
{
    [TestClass]
    public class ConstructorResolutionConfigurationTest
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
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void ExplicitPolicyDefaultConstructorWithoutAttribute()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.Explicit;

            Expect.ToThrow<CompositionException>(() => _context.GetComponent<ISampleContractA>());
        }

        [TestMethod]
        public void ExplicitPolicy()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(SingleNonDefaultConstructor));
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.Explicit;

            Expect.ToThrow<CompositionException>(() => _context.GetComponent<SingleNonDefaultConstructor>());
        }

        [TestMethod]
        public void AttributeOverridesPolicy()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructorsWithExplicit));

            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.DefaultConstructor;
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c = _context.GetComponent<ManyConstructorsWithExplicit>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(ManyConstructorsWithExplicit.ContractAAndBAndIntegerConstructor, c.InvokedConstructor);
            Assert.IsNotNull(c.ContractA);
            Assert.IsNotNull(c.ContractB);
            Assert.AreEqual(default, c.Integer);
            Assert.AreNotEqual(default, c.String);
        }

        [TestMethod]
        public void DefaultConstructorPolicy()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructors));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.DefaultConstructor;
            var c = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(ManyConstructors.DefaultConstructor, c.InvokedConstructor);
            Assert.IsNull(c.ContractA);
            Assert.IsNull(c.ContractB);
            Assert.AreNotEqual(default, c.Integer);
            Assert.AreNotEqual(default, c.String);
        }

        [TestMethod]
        public void SingleOrDefaultPolicyWithDefault()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructors));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.SingleOrDefault;
            var c = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(ManyConstructors.DefaultConstructor, c.InvokedConstructor);
            Assert.IsNull(c.ContractA);
            Assert.IsNull(c.ContractB);
            Assert.AreNotEqual(default, c.Integer);
            Assert.AreNotEqual(default, c.String);
        }

        [TestMethod]
        public void SingleOrDefaultPolicyWithoutDefaultSingleConstructor()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(SingleNonDefaultConstructor));

            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.SingleOrDefault;
            var c = _context.GetComponent<SingleNonDefaultConstructor>();

            Assert.IsNotNull(c);
            Assert.IsNotNull(c.A);
            Assert.IsNotNull(c.B);
            Assert.AreEqual(3, c.InvokedConstructor);
        }

        [TestMethod]
        public void SingleOrDefaultPolicyWithoutDefaultMultipleConstructors()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(NoDefaultMultipleUnmarkedConstructors));

            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.SingleOrDefault;

            Expect.ToThrow<CompositionException>(() => _context.GetComponent<NoDefaultMultipleUnmarkedConstructors>());
        }

        [TestMethod]
        public void MostParametersPolicy()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructors));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostParameters;
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(ManyConstructors.ContractAAndBAndIntegerAndStringConstructor, c.InvokedConstructor);
            Assert.IsNotNull(c.ContractA);
            Assert.IsNotNull(c.ContractB);
            Assert.AreEqual(default, c.Integer);
            Assert.AreEqual(default, c.String);
        }

        [TestMethod]
        public void MostParametersPolicyMultipleMatch()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(MultipleLeastAndMostParams));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostParameters;
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;

            Expect.ToThrow<CompositionException>(() => _context.GetComponent<MultipleLeastAndMostParams>());
        }

        [TestMethod]
        public void LeastParametersPolicyWithoutDefaultConstructor()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(NoDefaultMultipleUnmarkedConstructors));

            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.LeastParameters;

            var c = _context.GetComponent<NoDefaultMultipleUnmarkedConstructors>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(2, c.InvokedConstructor);
            Assert.IsNotNull(c.A);
            Assert.IsNull(c.B);
        }
        
        [TestMethod]
        public void LeastParametersPolicyWithDefaultConstructor()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructors));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.LeastParameters;
            var c = _context.GetComponent<ManyConstructors>();
            
            Assert.AreEqual(ManyConstructors.DefaultConstructor, c.InvokedConstructor);
            Assert.IsNull(c.ContractA);
            Assert.IsNull(c.ContractB);
            Assert.AreNotEqual(default, c.Integer);
            Assert.AreNotEqual(default, c.String);
        }

        [TestMethod]
        public void LeastParametersPolicyMultipleMatch()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(MultipleLeastAndMostParams));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.LeastParameters;
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;

            Expect.ToThrow<CompositionException>(() => _context.GetComponent<MultipleLeastAndMostParams>());
        }

        [TestMethod]
        public void MostResolvablePolicyRequiredParams()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructors));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostResolvable;
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            var c = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(ManyConstructors.ContractAAndBConstructor, c.InvokedConstructor);
            Assert.IsNotNull(c.ContractA);
            Assert.IsNotNull(c.ContractB);
            Assert.AreNotEqual(default, c.Integer);
            Assert.AreNotEqual(default, c.String);
        }

        [TestMethod]
        public void MostResolvablePolicyOptionalParams()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructors));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostResolvable;
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(ManyConstructors.ContractAAndBAndIntegerAndStringConstructor, c.InvokedConstructor);
            Assert.IsNotNull(c.ContractA);
            Assert.IsNotNull(c.ContractB);
            Assert.AreEqual(default, c.Integer);
            Assert.AreEqual(default, c.String);
        }

        [TestMethod]
        public void MostResolvablePolicyNoMatch()
        {
            _context.Register(typeof(NoDefaultMultipleUnmarkedConstructors));
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostResolvable;
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;

            Expect.ToThrow<CompositionException>(() => _context.GetComponent<NoDefaultMultipleUnmarkedConstructors>());
        }

        [TestMethod]
        public void MostResolvablePolicyMultipleMatch()
        {
            _context.Register(typeof(MultipleLeastAndMostParams));
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostResolvable;
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;

            var c = _context.GetComponent<MultipleLeastAndMostParams>();
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ContractA);
            Assert.IsNotNull(c.ContractB);
        }

        [TestMethod]
        public void CustomPolicy()
        {
            _context.Register(typeof(MultipleLeastAndMostParams));
            _context.Register(typeof(AnyConstructorWithMostParamsResolver));
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));

            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.Custom;
            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c = _context.GetComponent<MultipleLeastAndMostParams>();
            
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ContractA);
            Assert.IsNotNull(c.ContractB);
        }

        [TestMethod]
        public void SetPolicyWithAttribute()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(ComponentWithDirectPolicyMetadata));

            var c = _context.GetComponent<ComponentWithDirectPolicyMetadata>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(2, c.InvokedConstructor);
            Assert.IsNotNull(c.A);
            Assert.IsNull(c.B);
        }

        [TestMethod]
        public void ConstructorResolutionPolicyAttributeIsInherited()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(ComponentWithInheritedPolicyMetadata));

            var c = _context.GetComponent<ComponentWithInheritedPolicyMetadata>();
            
            Assert.IsNotNull(c);
            Assert.AreEqual(2, c.InvokedConstructor);
            Assert.IsNotNull(c.A);
            Assert.IsNull(c.B);
        }

        [TestMethod]
        public void ConstructorResolutionIsCachedPerRegistration()
        {
            _context.Register(typeof(SampleComponentA));
            _context.Register(typeof(SampleComponentB));
            _context.Register(typeof(ManyConstructors));
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.MostResolvable;
            
            
            _context.Configuration.ConstructorArgumentRequiredByDefault = true;
            var c1 = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c1);
            Assert.AreEqual(ManyConstructors.ContractAAndBConstructor, c1.InvokedConstructor);
            Assert.IsNotNull(c1.ContractA);
            Assert.IsNotNull(c1.ContractB);
            Assert.AreNotEqual(default, c1.Integer);
            Assert.AreNotEqual(default, c1.String);

            _context.Configuration.ConstructorArgumentRequiredByDefault = false;
            var c2 = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c2);
            Assert.AreEqual(ManyConstructors.ContractAAndBConstructor, c2.InvokedConstructor);
            Assert.IsNotNull(c2.ContractA);
            Assert.IsNotNull(c2.ContractB);
            Assert.AreNotEqual(default, c2.Integer);
            Assert.AreNotEqual(default, c2.String);

            _context.UnregisterFamily(typeof(ManyConstructors));
            _context.Register(typeof(ManyConstructors));
            var c3 = _context.GetComponent<ManyConstructors>();
            
            Assert.IsNotNull(c3);
            Assert.AreEqual(ManyConstructors.ContractAAndBAndIntegerAndStringConstructor, c3.InvokedConstructor);
            Assert.IsNotNull(c3.ContractA);
            Assert.IsNotNull(c3.ContractB);
            Assert.AreEqual(default, c3.Integer);
            Assert.AreEqual(default, c3.String);
        }
        
        [TestMethod]
        public void ConstructorResolutionIsNotCachedWhenFailed()
        {
            _context.Register(typeof(SampleComponentA));
            
            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.Explicit;
            Expect.ToThrow<CompositionException>(() => _context.GetComponent<ISampleContractA>());

            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.SingleOrDefault;
            Assert.IsNotNull(_context.GetComponent<ISampleContractA>());

            _context.Configuration.DefaultConstructorResolutionPolicy = ConstructorResolutionPolicy.Explicit;
            Assert.IsNotNull(_context.GetComponent<ISampleContractA>());
        }
    }
}