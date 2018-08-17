﻿using System;
using System.Reflection;
using ComposerCore.Implementation;
using ComposerCore.Tests.XmlValueParser.Components;
using ComposerCore.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.XmlValueParser
{
    [TestClass]
    public class OtherSimpleTest
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
        public void AttributeCharA()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("charA");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(char));

            var c = (char) o;
            Assert.AreEqual(c, 'A');
        }

        [TestMethod]
        public void AttributeCharTilde()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("char~");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(char));

            var c = (char) o;
            Assert.AreEqual(c, '~');
        }

        [TestMethod]
        public void AttributeDateTime()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("datetime");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(DateTime));

            var dt = (DateTime) o;
            Assert.AreEqual(dt, new DateTime(2010, 12, 22));
        }

        [TestMethod]
        public void AttributeString()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("string");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(string));

            var s = (string) o;
            Assert.AreEqual(s, "sampleString");
        }

        [TestMethod]
        public void AttributeByteArray()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("byteArray");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(byte[]));

            var bytes = (byte[]) o;
            Assert.IsTrue(bytes.Length == 32);

            for (int i = 0; i < 32; i++)
                Assert.AreEqual(bytes[i], i);
        }

        [TestMethod]
        public void AttributeEnumOne()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("enumOne");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SampleEnum));

            var e = (SampleEnum) o;
            Assert.AreEqual(e, SampleEnum.ValueOne);
        }

        [TestMethod]
        public void AttributeEnumTen()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("enumTen");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SampleEnum));

            var e = (SampleEnum) o;
            Assert.AreEqual(e, SampleEnum.ValueTen);
        }

        [TestMethod]
        public void AttributeRefWithoutName()
        {
            _context.Register(typeof(SampleComponent));
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("refWithoutName");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(ISampleContract));
        }

        [TestMethod]
        public void AttributeRefWithName()
        {
            _context.Register("sampleContractName", typeof(SampleComponent));
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("refWithName");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(ISampleContract));
        }

        [TestMethod]
        public void AttributeTypeInt()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("typeInt");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(Type));
            Assert.AreEqual(o, typeof(int));
        }

        [TestMethod]
        public void AttributeTypeISampleContract()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("typeISampleContract");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(Type));
            Assert.AreEqual(o, typeof(ISampleContract));
        }

        [TestMethod]
        public void AttributeAssembly()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("assembly");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(Assembly));
            Assert.AreEqual(o, typeof(ISampleContract).Assembly);
        }

        [TestMethod]
        public void AttributeContentsOfString()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("contentsOfString");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(string));

            var s = (string) o;
            Assert.AreEqual(s, "sampleString");
        }

        [TestMethod]
        public void AttributeContentsOfByteArray()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.AOtherSimple.xml");

            var o = _context.GetVariable("contentsOfByteArray");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(byte[]));

            var bytes = (byte[]) o;
            Assert.IsTrue(bytes.Length == 32);

            for (int i = 0; i < 32; i++)
                Assert.AreEqual(bytes[i], i);
        }

        [TestMethod]
        public void ElementCharA()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("charA");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(char));

            var c = (char) o;
            Assert.AreEqual(c, 'A');
        }

        [TestMethod]
        public void ElementCharTilde()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("char~");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(char));

            var c = (char) o;
            Assert.AreEqual(c, '~');
        }

        [TestMethod]
        public void ElementDateTime()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("datetime");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(DateTime));

            var dt = (DateTime) o;
            Assert.AreEqual(dt, new DateTime(2010, 12, 22));
        }

        [TestMethod]
        public void ElementString()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("string");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(string));

            var s = (string) o;
            Assert.AreEqual(s, "sampleString");
        }

        [TestMethod]
        public void ElementByteArray()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("byteArray");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(byte[]));

            var bytes = (byte[]) o;
            Assert.IsTrue(bytes.Length == 32);

            for (int i = 0; i < 32; i++)
                Assert.AreEqual(bytes[i], i);
        }

        [TestMethod]
        public void ElementNull()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("nullRef");

            Assert.IsNull(o);
        }

        [TestMethod]
        public void ElementComponentContext()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("componentContext");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(ComponentContext));
            Assert.AreSame(o, _context);
        }

        [TestMethod]
        public void ElementEnumOne()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("enumOne");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SampleEnum));

            var e = (SampleEnum) o;
            Assert.AreEqual(e, SampleEnum.ValueOne);
        }

        [TestMethod]
        public void ElementEnumTen()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("enumTen");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(SampleEnum));

            var e = (SampleEnum) o;
            Assert.AreEqual(e, SampleEnum.ValueTen);
        }

        [TestMethod]
        public void ElementRefWithoutName()
        {
            _context.Register(typeof(SampleComponent));
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("refWithoutName");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(ISampleContract));
        }

        [TestMethod]
        public void ElementRefWithName()
        {
            _context.Register("sampleContractName", typeof(SampleComponent));
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("refWithName");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(ISampleContract));
        }

        [TestMethod]
        public void ElementTypeInt()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("typeInt");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(Type));
            Assert.AreEqual(o, typeof(int));
        }

        [TestMethod]
        public void ElementTypeISampleContract()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("typeISampleContract");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(Type));
            Assert.AreEqual(o, typeof(ISampleContract));
        }

        [TestMethod]
        public void ElementAssembly()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("assembly");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(Assembly));
            Assert.AreEqual(o, typeof(ISampleContract).Assembly);
        }

        [TestMethod]
        public void ElementContentsOfString()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("contentsOfString");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(string));

            var s = (string) o;
            Assert.AreEqual(s, "sampleString");
        }

        [TestMethod]
        public void ElementContentsOfByteArray()
        {
            _context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
                "ComposerCore.Tests.XmlValueParser.Xmls.EOtherSimple.xml");

            var o = _context.GetVariable("contentsOfByteArray");

            Assert.IsNotNull(o);
            Assert.IsInstanceOfType(o, typeof(byte[]));

            var bytes = (byte[]) o;
            Assert.IsTrue(bytes.Length == 32);

            for (int i = 0; i < 32; i++)
                Assert.AreEqual(bytes[i], i);
        }
    }
}