using ComposerCore.Implementation;
using ComposerCore.Tests.ErrorConditions.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ErrorConditions
{
	[TestClass]
	public class ConfigurationErrors
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
		public void ConfigWithoutSetter()
		{
			_context.Register(typeof(ConfigPointWithoutSetter));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ConfigPointWithoutSetter>());
		}

		[TestMethod]
		public void ConfigWithPrivateSetter()
		{
			_context.Register(typeof(ConfigPointWithPrivateSetter));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ConfigPointWithPrivateSetter>());
		}
	}
}
