using ComposerCore.Implementation;
using ComposerCore.Tests.RequiredAndOptionalInitPoint.Components;
using ComposerCore.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint
{
    [TestClass]
    public class UnspecifiedConfigOptionalDefaultTest
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
			_context.Configuration.ConfigurationPointRequiredByDefault = false;
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
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.UnsConfigProvided.xml");

			var c = _context.GetComponent<ComponentWithUnspecifiedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void OptConfigNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedConfig));

			var c = _context.GetComponent<ComponentWithUnspecifiedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.SomeConfig);
		}

		[TestMethod]
		public void OptNamedConfigProvidedByVar()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedConfig));
			_context.SetVariableValue("someVariable", "SomeConfigValue");

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void OptNamedConfigProvidedByReg()
		{
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.UnsNamedConfigProvidedByReg.xml");

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.SomeConfig);
			Assert.AreEqual(c.SomeConfig, "SomeConfigValue");
		}

		[TestMethod]
		public void OptNamedConfigNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedConfig));

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedConfig>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.SomeConfig);
		}

		[TestMethod]
		public void DefaultIsEnforcedOnResolutionTime()
		{
			_context.Register(typeof(ComponentWithUnspecifiedConfig));

			var c1 = _context.GetComponent<ComponentWithUnspecifiedConfig>();
			Assert.IsNotNull(c1);
			Assert.IsNull(c1.SomeConfig);

			_context.Configuration.ConfigurationPointRequiredByDefault = true;
			
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedConfig>());
			
			_context.Configuration.ConfigurationPointRequiredByDefault = false;

			var c2 = _context.GetComponent<ComponentWithUnspecifiedConfig>();
			Assert.IsNotNull(c2);
			Assert.IsNull(c2.SomeConfig);
		}
    }
}