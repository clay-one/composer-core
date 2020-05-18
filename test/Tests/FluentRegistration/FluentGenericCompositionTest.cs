using ComposerCore.Attributes;
using ComposerCore.Cache;
using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Tests.FluentRegistration.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.FluentRegistration
{
    [TestClass]
    public class FluentGenericCompositionTest
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
        public void DirectClosedGeneric()
        {
            _context.ForComponent<ClosedGenericComponent>()
                .RegisterWith<IGenericContract<string>>();

            var c = _context.GetComponent<IGenericContract<string>>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void OpenGeneric()
        {
            _context.ForGenericComponent(typeof(OpenGenericComponent<>))
                .RegisterWith(typeof(IGenericContract<>));

            var c = _context.GetComponent<IGenericContract<string>>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void OpenGenericWithDifferentName()
        {
            _context.ForGenericComponent(typeof(OpenGenericComponentWithDifferentName<>))
                .RegisterWith(typeof(IGenericContract<>));

            var c = _context.GetComponent<IGenericContract<string>>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void DoubleOpenGeneric()
        {
            _context.ForGenericComponent(typeof(DoubleOpenGenericComponent<,>))
                .RegisterWith(typeof(IDoubleGenericContract<,>));

            var c = _context.GetComponent<IDoubleGenericContract<string, int>>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        [ExpectedException(typeof(CompositionException))]
        public void DoubleOpenGenericWithSingleParameterContract()
        {
            _context.ForGenericComponent(typeof(DoubleOpenGenericComponent<,>))
                .RegisterWith(typeof(IGenericContract<>));
        }

        [TestMethod]
        public void ReverseDoubleOpenGeneric()
        {
            _context.ForGenericComponent(typeof(ReverseDoubleOpenGenericComponent<,>))
                .RegisterWith(typeof(IDoubleGenericContract<,>));

            var c = _context.GetComponent<IDoubleGenericContract<string, int>>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void ReverseDoubleOpenGenericComponentWithDifferentNames()
        {
            _context.ForGenericComponent(typeof(ReverseDoubleOpenGenericComponentWithDifferentNames<,>))
                .RegisterWith(typeof(IDoubleGenericContract<,>));
            
            var c = _context.GetComponent<IDoubleGenericContract<string, int>>();

            Assert.IsNotNull(c);
        }

        [TestMethod]
        public void OpenGenericWithSpecificTypeParams()
        {
            _context.ForComponent<OpenGenericComponent<string>>()
                .RegisterWith<IGenericContract<string>>();
             
            _context.ForComponent(typeof(OpenGenericComponent<int>))
                .RegisterWith(typeof(IGenericContract<int>));
            
            var c1 = _context.GetComponent<IGenericContract<string>>();
            var c2 = _context.GetComponent<IGenericContract<int>>();
            var c3 = _context.GetComponent<IGenericContract<long>>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNull(c3);
        }
        
        [TestMethod]
        [ExpectedException(typeof(CompositionException))]
        public void IncompatibleGenericTypeInContract()
        {
            _context.ForComponent<ClosedGenericComponent>()
                .RegisterWith<IGenericContract<int>>();
        }
        
        [TestMethod]
        public void RegisterWithDefaultCache()
        {
            _context
                .ForGenericComponent(typeof(OpenGenericComponent<>))
                .RegisterWith(typeof(IGenericContract<>));

            var c1 = _context.GetComponent<IGenericContract<string>>();
            var c2 = _context.GetComponent<IGenericContract<string>>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsTrue(ReferenceEquals(c1, c2));
        }

        [TestMethod]
        public void RegisterWithContractAgnosticCache()
        {
            _context
                .ForGenericComponent(typeof(OpenGenericComponent<>))
                .UseComponentCache<ContractAgnosticComponentCache>()
                .RegisterWith(typeof(IGenericContract<>));

            var c1 = _context.GetComponent<IGenericContract<string>>();
            var c2 = _context.GetComponent<IGenericContract<string>>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsTrue(ReferenceEquals(c1, c2));
        }

        [TestMethod]
        public void RegisterWithNoCache()
        {
            _context
                .ForGenericComponent(typeof(OpenGenericComponent<>))
                .UseComponentCache(null)
                .RegisterWith(typeof(IGenericContract<>));

            var c1 = _context.GetComponent<IGenericContract<string>>();
            var c2 = _context.GetComponent<IGenericContract<string>>();

            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsFalse(ReferenceEquals(c1, c2));
        }

        [TestMethod]
        public void RegisterWithConstructorResolutionPolicy()
        {
            _context.Register(typeof(ComponentOne));
            _context
                .ForGenericComponent(typeof(GenericComponentWithManyConstructors<>))
                .SetConstructorResolutionPolicy(ConstructorResolutionPolicy.MostResolvable)
                .RegisterWith(typeof(IGenericContract<>));

            var i = _context.GetComponent<IGenericContract<string>>();
            var c = i as GenericComponentWithManyConstructors<string>;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.One);
            Assert.IsNull(c.Two);
            Assert.AreSame(GenericComponentWithManyConstructors<string>.ContractAConstructor, c.InvokedConstructor);
        }
    }
}