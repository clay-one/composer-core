﻿using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Tests.FluentRegistration.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.FluentRegistration
{
    [TestClass]
    public class FluentAddCompositionNotificationTest
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
        public void AddInitializationMethod()
        {
            _context.ForComponent<NonAttributedComponent>()
                .NotifyInitialized((cmpsr, x) => x.Initialize())
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsTrue(c.InitCalled);
        }

        [TestMethod]
        public void AddInitializationDelegate()
        {
            _context.ForComponent<NonAttributedComponent>()
                .NotifyInitialized((cmpsr, x) => x.ParameterizedInit(5))
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.AreEqual(5, c.InitValue);
        }

        [TestMethod]
        public void AddMultipleInitializations()
        {
            _context.ForComponent<NonAttributedComponent>()
                .NotifyInitialized((cmpsr, x) => x.Initialize())
                .NotifyInitialized((cmpsr, x) => x.Initialize2())
                .NotifyInitialized((cmpsr, x) => x.ParameterizedInit(5))
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsTrue(c.InitCalled);
            Assert.IsTrue(c.InitCalled2);
            Assert.AreEqual(5, c.InitValue);
        }
    }
}