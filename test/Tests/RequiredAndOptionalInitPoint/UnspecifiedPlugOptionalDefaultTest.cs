using ComposerCore.Implementation;
using ComposerCore.Tests.RequiredAndOptionalInitPoint.Components;
using ComposerCore.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint
{
    [TestClass]
    public class UnspecifiedPlugOptionalDefaultTest
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
			_context.Configuration.ComponentPlugRequiredByDefault = false;
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void UnsPlugProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedPlug));
			_context.Register(typeof(PluggedComponent));

			var c = _context.GetComponent<ComponentWithUnspecifiedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.PluggedContract);
		}

		[TestMethod]
		public void UnsPlugProvidedWithDifferentName()
		{
			_context.Register(typeof(ComponentWithUnspecifiedPlug));
			_context.Register("someOtherName", typeof(PluggedComponent));

			var c = _context.GetComponent<ComponentWithUnspecifiedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.PluggedContract);
		}

		[TestMethod]
		public void UnsPlugNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedPlug));

			var c = _context.GetComponent<ComponentWithUnspecifiedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.PluggedContract);
		}

		[TestMethod]
		public void UnsNamedPlugProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));
			_context.Register("contractName", typeof(PluggedComponent));

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.PluggedContract);
		}

		[TestMethod]
		public void UnsNamedPlugProvidedWithDifferentName()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));
			_context.Register("someOtherName", typeof(PluggedComponent));

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.PluggedContract);
		}

		[TestMethod]
		public void UnsNamedPlugProvidedWithoutName()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));
			_context.Register(typeof(PluggedComponent));

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.PluggedContract);
		}

		[TestMethod]
		public void UnsNamedPlugNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNull(c.PluggedContract);
		}

		[TestMethod]
		public void UnsNamedPlugRedirectedUponRegistration()
		{
			_context.Register("someOtherName", typeof(PluggedComponent));
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.UnsNamedPlugRedirected.xml");

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.PluggedContract);
		}

		[TestMethod]
		public void DefaultIsEnforcedOnResolutionTime()
		{
			_context.Register(typeof(ComponentWithUnspecifiedPlug));

			var c1 = _context.GetComponent<ComponentWithUnspecifiedPlug>();
			Assert.IsNotNull(c1);
			Assert.IsNull(c1.PluggedContract);

			_context.Configuration.ComponentPlugRequiredByDefault = true;
			
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedPlug>());
			
			_context.Configuration.ComponentPlugRequiredByDefault = false;
			
			var c2 = _context.GetComponent<ComponentWithUnspecifiedPlug>();
			Assert.IsNotNull(c2);
			Assert.IsNull(c2.PluggedContract);
		}
    }
}