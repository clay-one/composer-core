using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ComposerCore.Implementation;
using ComposerCore.Tests.ComponentCaching.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ComponentCaching
{
	[TestClass]
	public class UncachedTest
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
			_context.Register(typeof(UncachedComponent));
			_context.Register(typeof(UncachedComponentWithPlugs));
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
		public void RegisterTwoTimesQueryBySelf()
		{
			_context.Register(typeof(UncachedComponent)); // Register for second time

			var all = _context.GetAllComponents<UncachedComponent>();
			Assert.IsNotNull(all);
			Assert.IsTrue(all.Count() == 2);

			var allArray = all.ToArray();
			var c0 = allArray[0];
			var c1 = allArray[1];
			Assert.AreNotSame(c0, c1);
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
		public void RegisterTwoTimesQueryByContract()
		{
			_context.Register(typeof(UncachedComponent));

			var all = _context.GetAllComponents<ISomeContract>();
			Assert.IsNotNull(all);
			Assert.IsTrue(all.Count() == 2);

			var allArray = all.ToArray();
			var c0 = allArray[0];
			var c1 = allArray[1];
			Assert.AreNotSame(c0, c1);
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
		public void CheckDifferentContracts()
		{
			var c0 = _context.GetComponent<ISomeContract>();
			var c1 = _context.GetComponent<IAnotherContract>();
			var c2 = _context.GetComponent<UncachedComponent>();

			Assert.AreNotSame(c0, c1);
			Assert.AreNotSame(c0, c2);
			Assert.AreNotSame(c1, c2);
		}

		[TestMethod]
		public void QueryTwoTimesByContract()
		{
			var c0 = _context.GetComponent<ISomeContract>();
			var c1 = _context.GetComponent<ISomeContract>();

			Assert.AreNotSame(c0, c1);
		}

		[TestMethod]
		public void QueryTwoTimesBySelf()
		{
			var c0 = _context.GetComponent<UncachedComponent>();
			var c1 = _context.GetComponent<UncachedComponent>();

			Assert.AreNotSame(c0, c1);
		}

		[TestMethod]
		[SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
		public void QueryTwoTimesIndirect()
		{
			_context.Register(typeof(UncachedComponentWithPlugs));
			_context.Register(typeof(ContractAgnosticComponent));
			_context.Register(typeof(DefaultCacheComponent));
			_context.Register(typeof(ScopedComponent));

			var all = _context.GetAllComponents<UncachedComponentWithPlugs>();
			Assert.IsNotNull(all);
			Assert.IsTrue(all.Count() == 2);

			var allArray = all.ToArray();
			var c0 = allArray[0];
			var c1 = allArray[1];
			Assert.AreNotSame(c0, c1);
			Assert.AreNotSame(c0.UncachedComponent, c1.UncachedComponent);
		}
	}
}
