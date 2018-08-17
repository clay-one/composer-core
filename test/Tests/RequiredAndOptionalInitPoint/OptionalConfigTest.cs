﻿using ComposerCore.Implementation;
using ComposerCore.Tests.RequiredAndOptionalInitPoint.Components;
using ComposerCore.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint
{
	[TestClass]
	public class OptionalConfigTest
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
		public void OptConfigProvided()
		{
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.OptConfigProvided.xml");

			var c = _context.GetComponent<ComponentWithOptionalConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void OptConfigNotProvided()
		{
			_context.Register(typeof(ComponentWithOptionalConfig));

			var c = _context.GetComponent<ComponentWithOptionalConfig>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.SomeConfig);
		}

		[TestMethod]
		public void OptNamedConfigProvidedByVar()
		{
			_context.Register(typeof(ComponentWithOptionalNamedConfig));
			_context.SetVariableValue("someVariable", "SomeConfigValue");

			var c = _context.GetComponent<ComponentWithOptionalNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void OptNamedConfigProvidedByReg()
		{
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.OptNamedConfigProvidedByReg.xml");

			var c = _context.GetComponent<ComponentWithOptionalNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void OptNamedConfigNotProvided()
		{
			_context.Register(typeof(ComponentWithOptionalNamedConfig));

			var c = _context.GetComponent<ComponentWithOptionalNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.SomeConfig);
		}
	}
}
