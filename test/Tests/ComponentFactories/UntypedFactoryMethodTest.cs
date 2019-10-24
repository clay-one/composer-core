using System;
using System.Collections.Generic;
using ComposerCore.Cache;
using ComposerCore.CompositionalQueries;
using ComposerCore.Factories;
using ComposerCore.Implementation;
using ComposerCore.Tests.ComponentFactories.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ComponentFactories
{
    [TestClass]
    public class UntypedFactoryMethodTest
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
        public void RegisterWithoutContractSpecification()
        {
            var factory = new UntypedFactoryMethodComponentFactory(composer => new SampleComponentOne());
            Expect.ToThrow<CompositionException>(() => _context.Register(factory));
        }
        
        [TestMethod]
        public void SimpleRegistrationAndLookup()
        {
            _context.Register(new UntypedFactoryMethodComponentFactory(
                composer => new SampleComponentOne(),
                new[] {typeof(ISampleContractOne)}));

            Assert.IsTrue(_context.IsResolvable<ISampleContractOne>());
            Assert.IsNotNull(_context.GetComponent<ISampleContractOne>());
        }

        [TestMethod]
        public void UseConstructorParameterClosure()
        {
            var guid = Guid.NewGuid().ToString();
            _context.Register(new UntypedFactoryMethodComponentFactory(
                composer => new SampleComponentOne(guid),
                new[] {typeof(ISampleContractOne)}));

            Assert.IsTrue(_context.IsResolvable<ISampleContractOne>());
            
            var c = _context.GetComponent<ISampleContractOne>() as SampleComponentOne;
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ConstructorArg);
            Assert.AreSame(guid, c.ConstructorArg);
        }

        [TestMethod]
        public void InitializePropertyInFactoryMethod()
        {
            var guid = Guid.NewGuid().ToString();
            _context.Register(new UntypedFactoryMethodComponentFactory(
                composer => new SampleComponentOne { PublicProperty = guid },
                new[] {typeof(ISampleContractOne)}));

            Assert.IsTrue(_context.IsResolvable<ISampleContractOne>());
            
            var c = _context.GetComponent<ISampleContractOne>() as SampleComponentOne;
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.PublicProperty);
            Assert.AreSame(guid, c.PublicProperty);
        }

        [TestMethod]
        public void RegisterAsTransient()
        {
            var factory = new UntypedFactoryMethodComponentFactory(composer => new SampleComponentOne());
            factory.ContractTypes = new List<Type> {typeof(ISampleContractOne)};
            
            var registration = new ConcreteComponentRegistration(factory);
            registration.SetCache(NoComponentCache.Instance);
            _context.Register(registration);

            var originalInstanceCount = SampleComponentOne.TimesInstantiated;
            
            var c1 = _context.GetComponent<ISampleContractOne>();
            var c2 = _context.GetComponent<ISampleContractOne>();
            var c3 = _context.GetComponent<ISampleContractOne>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreNotSame(c1, c2);
            Assert.AreNotSame(c1, c3);
            Assert.AreNotSame(c2, c3);
            
            Assert.AreEqual(originalInstanceCount + 3, SampleComponentOne.TimesInstantiated);
        }

        [TestMethod]
        public void RegisterAsSingleton()
        {
            var factory = new UntypedFactoryMethodComponentFactory(composer => new SampleComponentOne());
            factory.ContractTypes = new List<Type> {typeof(ISampleContractOne)};

            var registration = new ConcreteComponentRegistration(factory);
            registration.SetCache(nameof(ContractAgnosticComponentCache));
            _context.Register(registration);

            var originalInstanceCount = SampleComponentOne.TimesInstantiated;

            var c1 = _context.GetComponent<ISampleContractOne>();
            var c2 = _context.GetComponent<ISampleContractOne>();
            var c3 = _context.GetComponent<ISampleContractOne>();
            
            Assert.IsNotNull(c1);
            Assert.IsNotNull(c2);
            Assert.IsNotNull(c3);
            
            Assert.AreSame(c1, c2);
            Assert.AreSame(c1, c3);
            Assert.AreSame(c2, c3);
            
            Assert.AreEqual(originalInstanceCount + 1, SampleComponentOne.TimesInstantiated);
        }

        [TestMethod]
        public void PlugsAreInitialized()
        {
            _context.Register(typeof(SampleComponentOne));
            
            var factory = new UntypedFactoryMethodComponentFactory(
                composer => new SampleComponentTwo(),
                new[] {typeof(ISampleContractTwo)});
            _context.Register(factory);

            var c = _context.GetComponent<ISampleContractTwo>() as SampleComponentTwo;
            
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.One);
        }

        [TestMethod]
        public void UnresolvableRequiredPlugs()
        {
            var factory = new UntypedFactoryMethodComponentFactory(
                composer => new SampleComponentTwo(),
                new[] {typeof(ISampleContractTwo)});
            _context.Register(factory);

            Expect.ToThrow<CompositionException>(() => _context.GetComponent<ISampleContractTwo>());
        }

        [TestMethod]
        public void RegisterWithInvalidContract()
        {
            var factory = new UntypedFactoryMethodComponentFactory(
                composer => new SampleComponentOne(),
                new[] {typeof(ISampleContractOne)});
            
            // The type check cannot be performed at the time of registration (because it is "Untyped")
            _context.Register(typeof(ISampleContractTwo), factory);
            
            // But a type cast exception will be thrown when querying
            Expect.ToThrow<InvalidCastException>(() => _context.GetComponent<ISampleContractTwo>());
        }
    }
}