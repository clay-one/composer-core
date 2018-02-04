using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComposerCore.Tests.InitializationPointVariety.Components;
using ComposerCore.Utility;

namespace ComposerCore.Tests.InitializationPointVariety
{
	[TestClass]
	public class ResourcePlugTest
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
			_context.ProcessCompositionXmlFromResource("ComposerCore.Tests.InitializationPointVariety.Xmls.ResourceComposition.xml");
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void ResourcePlugAsField()
		{
			_context.Register(typeof(WithFieldResourcePlug));

			var c = _context.GetComponent<WithFieldResourcePlug>();

			Assert.IsNotNull(c.ResourcePlug);
		}

		[TestMethod]
		public void ResourcePlugAsProperty()
		{
			_context.Register(typeof(WithPropertyResourcePlug));

			var c = _context.GetComponent<WithPropertyResourcePlug>();

			Assert.IsNotNull(c.ResourcePlug);
		}
	}
}
