using ComposerCore.Implementation;
using ComposerCore.Tests.CompositionByConstructor.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.CompositionByConstructor
{
	[TestClass]
	public class ErrorConstructorLogicTest
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
			_context.Register(typeof(SampleComponentA));
			_context.Register(typeof(SampleComponentB));
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void PrivateDefaultConstructorThrows()
		{
			_context.Register(typeof(PrivateDefaultSingleConstructor));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<PrivateDefaultSingleConstructor>());
		}

		[TestMethod]
		public void PrivateDefaultWithOtherUnmarkedConstructorsThrows()
		{
			_context.Register(typeof(PrivateDefaultNoneMarkedFromOtherConstructors));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<PrivateDefaultNoneMarkedFromOtherConstructors>());
		}

		[TestMethod]
		public void PrivateMarkedConstructorThrows()
		{
			_context.Register(typeof(PrivateMarkedConstructor));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<PrivateMarkedConstructor>());
		}

		[TestMethod]
		public void MultipleMarkedConstructorsThrows()
		{
			_context.Register(typeof(MultipleMarkedConstructors));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<MultipleMarkedConstructors>());
		}

		[TestMethod]
		public void MultipleNonDefaultNoneMarkedThrows()
		{
			_context.Register(typeof(NoDefaultMultipleUnmarkedConstructors));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<NoDefaultMultipleUnmarkedConstructors>());
		}

		[TestMethod]
		public void ConstructorParameterAreRequired()
		{
			_context.UnregisterFamily(typeof(ISampleContractA));
			_context.Register(typeof(MultiConstructorsWithDefaultAndMarked));

			Expect.ToThrow<CompositionException>(() => _context.GetComponent<MultiConstructorsWithDefaultAndMarked>());
		}

		[TestMethod]
		public void NonContractParameterThrows()
		{
			_context.Register(typeof(ConstructorWithIntegerParam));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ConstructorWithIntegerParam>());
		}

		[TestMethod]
		public void ExtraNamesThrows()
		{
			_context.Register(typeof(NamesMoreThanParamCount));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<NamesMoreThanParamCount>());
		}
	}
}
