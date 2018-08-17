﻿using ComposerCore.FluentExtensions;
using ComposerCore.Implementation;
using ComposerCore.Tests.FluentRegistration.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.FluentRegistration
{
    [TestClass]
    public class FluentSetComponentTest
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

            _context.Register(typeof(ComponentOne));
            _context.Register(typeof(ComponentTwo));
            _context.Register(typeof(IComponentOne), "name", typeof(ComponentOne));
            _context.Register(typeof(IComponentTwo), "name", typeof(ComponentTwo));
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void QueryComponentPlugs()
        {
            _context.ForComponent<NonAttributedComponent>()
                .SetComponent(x => x.ComponentOne)
                .SetComponent(x => x.ComponentTwo)
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNotNull(c.ComponentTwo);
        }

        [TestMethod]
        public void QueryComponentPlugsWithName()
        {
            _context.ForComponent<NonAttributedComponent>()
                .SetComponent(x => x.ComponentOne, "name")
                .SetComponent(x => x.ComponentTwo, "name")
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNotNull(c.ComponentOne);
            Assert.IsNotNull(c.ComponentTwo);
        }

        [TestMethod]
        public void QueryOptionalComponentPlugs()
        {
            _context.ForComponent<NonAttributedComponent>()
                .SetComponent(x => x.ComponentOne, "unknown", false)
                .SetComponent(x => x.ComponentTwo, "unknown", false)
                .RegisterWith<INonAttributedContract>();

            var i = _context.GetComponent<INonAttributedContract>();
            var c = i as NonAttributedComponent;

            Assert.IsNotNull(i);
            Assert.IsNotNull(c);
            Assert.IsNull(c.ComponentOne);
            Assert.IsNull(c.ComponentTwo);
        }
    }
}