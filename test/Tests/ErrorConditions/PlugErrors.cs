using ComposerCore.Implementation;
using ComposerCore.Tests.ErrorConditions.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ErrorConditions
{
	[TestClass]
	public class PlugErrors
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
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void PlugWithoutSetter()
		{
			_context.Register(typeof(PlugWithoutSetter));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<PlugWithoutSetter>());
		}

		[TestMethod]
		public void PlugWithPrivateSetter()
		{
			_context.Register(typeof(PlugWithPrivateSetter));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<PlugWithPrivateSetter>());
		}

		[TestMethod]
		public void PlugWithNonContractType()
		{
			_context.Register(typeof(NonContractPlugType));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<NonContractPlugType>());
		}
	}
}
