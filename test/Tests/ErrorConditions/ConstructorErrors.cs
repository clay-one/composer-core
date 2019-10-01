using ComposerCore.Implementation;
using ComposerCore.Tests.ErrorConditions.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ErrorConditions
{
	[TestClass]
	public class ConstructorErrors
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
		public void MultipleCompConstructors()
		{
			_context.Register(typeof(MultipleCompositionConstructors));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent(typeof(MultipleCompositionConstructors)));
		}

		[TestMethod]
		public void NoCompConstructors()
		{
			_context.Register(typeof(NoCompositionContructors));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent(typeof(NoCompositionContructors)));
		}

		[TestMethod]
		public void PrivateCompConstructor()
		{
			_context.Register(typeof(PrivateCompositionConstructor));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent(typeof(PrivateCompositionConstructor)));
		}

		[TestMethod]
		public void NonContractParam()
		{
			_context.Register(typeof(NonContractConstructorArg));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent(typeof(NonContractConstructorArg)));
		}
	}
}
