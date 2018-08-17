using System;
using ComposerCore.Implementation;
using ComposerCore.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.XmlValueParser
{
    [TestClass]
    public class SingleTest
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
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

        [TestMethod]
        public void SingleAttribute()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ASingle.xml");

            var o = _context.GetVariable("singlepi");

            Assert.IsNotNull(o);

            Assert.IsInstanceOfType(o, typeof(float));

            var d = (float) o;

            Assert.AreEqual(d, (float) 3.141593);
        }

        [TestMethod]
        public void SingleAttributeNaN()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ASingle.xml");

            var o = _context.GetVariable("singleNaN");

            Assert.IsNotNull(o);

            Assert.IsInstanceOfType(o, typeof(float));

            var d = (float) o;

            Assert.AreEqual(d, float.NaN);
        }

        [TestMethod]
        public void SingleAttributeZero()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ASingle.xml");

            var oPlus = _context.GetVariable("single+0");
            var oMinus = _context.GetVariable("single-0");

            Assert.IsNotNull(oPlus);
            Assert.IsNotNull(oMinus);

            Assert.IsInstanceOfType(oPlus, typeof(float));
            Assert.IsInstanceOfType(oMinus, typeof(float));

            var dPlus = (float) oPlus;
            var dMinus = (float) oMinus;

            Assert.AreEqual(dPlus, +0);
            Assert.AreEqual(dMinus, -0);
            Assert.AreEqual(dPlus, dMinus);
        }

        [TestMethod]
        public void SingleAttributeInfinity()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ASingle.xml");

            var oPlus = _context.GetVariable("single+inf");
            var oMinus = _context.GetVariable("single-inf");

            Assert.IsNotNull(oPlus);
            Assert.IsNotNull(oMinus);

            Assert.IsInstanceOfType(oPlus, typeof(float));
            Assert.IsInstanceOfType(oMinus, typeof(float));

            var dPlus = (float) oPlus;
            var dMinus = (float) oMinus;

            Assert.AreEqual(dPlus, Single.PositiveInfinity);
            Assert.AreEqual(dMinus, Single.NegativeInfinity);
        }

        [TestMethod]
        public void SingleElement()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ESingle.xml");

            var o = _context.GetVariable("singlepi");

            Assert.IsNotNull(o);

            Assert.IsInstanceOfType(o, typeof(float));

            var d = (float) o;

            Assert.AreEqual(d, (float) 3.141593);
        }

        [TestMethod]
        public void SingleElementNaN()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ESingle.xml");

            var o = _context.GetVariable("singleNaN");

            Assert.IsNotNull(o);

            Assert.IsInstanceOfType(o, typeof(float));

            var d = (float) o;

            Assert.AreEqual(d, float.NaN);
        }

        [TestMethod]
        public void SingleElementZero()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ESingle.xml");

            var oPlus = _context.GetVariable("single+0");
            var oMinus = _context.GetVariable("single-0");

            Assert.IsNotNull(oPlus);
            Assert.IsNotNull(oMinus);

            Assert.IsInstanceOfType(oPlus, typeof(float));
            Assert.IsInstanceOfType(oMinus, typeof(float));

            var dPlus = (float) oPlus;
            var dMinus = (float) oMinus;

            Assert.AreEqual(dPlus, +0);
            Assert.AreEqual(dMinus, -0);
            Assert.AreEqual(dPlus, dMinus);
        }

        [TestMethod]
        public void SingleElementInfinity()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.ESingle.xml");

            var oPlus = _context.GetVariable("single+inf");
            var oMinus = _context.GetVariable("single-inf");

            Assert.IsNotNull(oPlus);
            Assert.IsNotNull(oMinus);

            Assert.IsInstanceOfType(oPlus, typeof(float));
            Assert.IsInstanceOfType(oMinus, typeof(float));

            var dPlus = (float) oPlus;
            var dMinus = (float) oMinus;

            Assert.AreEqual(dPlus, Single.PositiveInfinity);
            Assert.AreEqual(dMinus, Single.NegativeInfinity);
        }
    }
}