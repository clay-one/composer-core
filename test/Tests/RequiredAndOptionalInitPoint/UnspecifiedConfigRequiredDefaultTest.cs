using ComposerCore.Implementation;
using ComposerCore.Tests.RequiredAndOptionalInitPoint.Components;
using ComposerCore.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint
{
	[TestClass]
    public class UnspecifiedConfigRequiredDefaultTest
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
			_context.Configuration.ConfigurationPointRequiredByDefault = true;
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void ReqConfigProvided()
		{
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.UnsConfigProvided.xml");

			var c = _context.GetComponent<ComponentWithUnspecifiedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void ReqConfigNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedConfig));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedConfig>());
		}

		[TestMethod]
		public void ReqNamedConfigProvidedByVar()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedConfig));
			_context.SetVariableValue("someVariable", "SomeConfigValue");

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void ReqNamedConfigProvidedByReg()
		{
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.UnsNamedConfigProvidedByReg.xml");

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void ReqNamedConfigNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedConfig));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedNamedConfig>());
		}
    }
}