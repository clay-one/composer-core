﻿using ComposerCore.Implementation;
using ComposerCore.Tests.ProvidedContractVariety.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.ProvidedContractVariety
{
	[TestClass]
	public class ProvidedContractTest
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
			_context.Register(typeof(InterfaceContractComponent));
			_context.Register(typeof(AbstractContractComponent));
			_context.Register(typeof(ConcreteContractComponent));
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void InterfaceContractTest()
		{
			var c = _context.GetComponent<IInterfaceContract>();
			Assert.IsNotNull(c);
			Assert.IsInstanceOfType(c, typeof(InterfaceContractComponent));
		}

		[TestMethod]
		public void AbstractContractTest()
		{
			var c = _context.GetComponent<AbstractContract>();
			Assert.IsNotNull(c);
			Assert.IsInstanceOfType(c, typeof(AbstractContractComponent));
		}

		[TestMethod]
		public void ConcreteContractTest()
		{
			var c = _context.GetComponent<ConcreteContract>();
			Assert.IsNotNull(c);
			Assert.IsInstanceOfType(c, typeof(ConcreteContractComponent));
		}

		[TestMethod]
		public void SelfContractWithoutExplicitContractRegistrationTest()
		{
		    _context.Register(typeof(SelfContractComponent));

            var c = _context.GetComponent<SelfContractComponent>();
			Assert.IsNotNull(c);
		}

		[TestMethod]
		public void SelfContractWithExplicitContractRegistrationTest()
		{
		    _context.Register(typeof(SelfContractComponent), typeof(SelfContractComponent));

            var c = _context.GetComponent<SelfContractComponent>();
			Assert.IsNotNull(c);
		}
	}
}
