using System.Linq;
using ComposerCore.Definitions.Utility;
using ComposerCore.Tests.LazyTests.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.LazyTests
{
	[TestClass]
	public class LazyGetComponentFamilyTest
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
		public void GenericGetComponentFamily()
		{
			var cLazy = _context.LazyGetComponentFamily<ISampleContract>();
			_context.Register(typeof(SampleComponent));

			var c = cLazy.Value;

			Assert.IsNotNull(c);
			Assert.AreEqual(1, c.Count());
		}

		[TestMethod]
		public void GetComponentFamily()
		{
			var cLazy = _context.LazyGetComponentFamily(typeof(ISampleContract));
			_context.Register(typeof(SampleComponent));

			var c = cLazy.Value;

			Assert.IsNotNull(c);
			Assert.AreEqual(1, c.Count());
		}
	}
}
