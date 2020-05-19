using ComposerCore.Implementation;
using ComposerCore.Tests.ErrorConditions.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ErrorConditions
{
	[TestClass]
	public class ResourceErrors
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
		public void ResourcePlugWithoutName()
		{
			_context.Register(typeof(ResourcePlugWithoutName));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ResourcePlugWithoutName>());
		}

		[TestMethod]
		public void ResourcePlugWithWrongType()
		{
			_context.Register(typeof(ResourcePlugWithWrongType));
			Expect.ToThrow<CompositionException>(() => _context.GetComponent<ResourcePlugWithWrongType>());
		}
	}
}
