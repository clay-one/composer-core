using ComposerCore.Implementation;
using ComposerCore.Tests.RequiredAndOptionalInitPoint.Components;
using ComposerCore.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.RequiredAndOptionalInitPoint
{
    [TestClass]
    public class UnspecifiedPlugRequiredDefaultTest
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
			_context.Configuration.InitializationPointsRequiredByDefault = true;
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void ReqPlugProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedPlug));
			_context.Register(typeof(PluggedComponent));

			var c = _context.GetComponent<ComponentWithUnspecifiedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.PluggedContract);
		}

		[TestMethod]
		public void ReqPlugProvidedWithDifferentName()
		{
			_context.Register(typeof(ComponentWithUnspecifiedPlug));
			_context.Register("someOtherName", typeof(PluggedComponent));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedPlug>());
		}

		[TestMethod]
		public void ReqPlugNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedPlug));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedPlug>());
		}

		[TestMethod]
		public void ReqNamedPlugProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));
			_context.Register("contractName", typeof(PluggedComponent));

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.PluggedContract);
		}

		[TestMethod]
		public void ReqNamedPlugProvidedWithDifferentName()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));
			_context.Register("someOtherName", typeof(PluggedComponent));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedNamedPlug>());
		}

		[TestMethod]
		public void ReqNamedPlugProvidedWithoutName()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));
			_context.Register(typeof(PluggedComponent));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedNamedPlug>());
		}

		[TestMethod]
		public void ReqNamedPlugNotProvided()
		{
			_context.Register(typeof(ComponentWithUnspecifiedNamedPlug));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ComponentWithUnspecifiedNamedPlug>());
		}

		[TestMethod]
		public void ReqNamedPlugRedirectedUponRegistration()
		{
			_context.Register("someOtherName", typeof(PluggedComponent));
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly,
				"ComposerCore.Tests.RequiredAndOptionalInitPoint.Xmls.UnsNamedPlugRedirected.xml");

			var c = _context.GetComponent<ComponentWithUnspecifiedNamedPlug>();

			Assert.IsNotNull(c);
			Assert.IsNotNull(c.PluggedContract);
		}
    }
}