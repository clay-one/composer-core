﻿using System.Runtime.CompilerServices;
using ComposerCore.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComposerCore.Tests.CompositionNotification.Components;
using ComposerCore.Utility;

namespace ComposerCore.Tests.CompositionNotification
{
	[TestClass]
	public class GetComponentTest
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
		[MethodImpl(MethodImplOptions.NoInlining)]
		public void TestInitialize()
		{
			_context = new ComponentContext();
			_context.ProcessCompositionXmlFromResource(typeof(AssemblyPointer).Assembly, 
				"ComposerCore.Tests.CompositionNotification.Xmls.Composition.xml");
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void ImplicitInterfaceImpl()
		{
			var c = _context.GetComponent<ImplicitInterfaceImpl>();
			Assert.IsTrue(c.HasInterfaceImplBeenCalled);
		}

		[TestMethod]
		public void ExplicitInterfaceImpl()
		{
			var c = _context.GetComponent<ExplicitInterfaceImpl>();
			Assert.IsTrue(c.HasInterfaceImplBeenCalled);
		}

		[TestMethod]
		public void SingleAttributedMethod()
		{
			var c = _context.GetComponent<SingleAttributedMethod>();
			Assert.IsTrue(c.HasAttributedMethodBeenCalled);
		}

		[TestMethod]
		public void MultipleAttributedMethods()
		{
			var c = _context.GetComponent<MultipleAttributedMethods>();
			Assert.IsTrue(c.HasAttributedMethodBeenCalledOne);
			Assert.IsTrue(c.HasAttributedMethodBeenCalledTwo);
			Assert.IsTrue(c.HasAttributedMethodBeenCalledThree);
		}

		[TestMethod]
		public void NonOverlappingAttribAndInterface()
		{
			var c = _context.GetComponent<NonOverlappingAttribAndInterface>();

			Assert.IsTrue(c.HasInterfaceImplBeenCalled);
			Assert.IsTrue(c.HasAttributedMethodBeenCalledOne);
			Assert.IsTrue(c.HasAttributedMethodBeenCalledTwo);

			Assert.IsFalse(c.HasInterfaceImplBeenCalledTwice);
			Assert.IsTrue(c.HasInterfaceImplBeenCalledAfterAllAttribs);
		}

		[TestMethod]
		public void OverlappingAttribAndInterface()
		{
			var c = _context.GetComponent<OverlappingAttribAndInterface>();

			Assert.IsTrue(c.HasInterfaceImplBeenCalled);
			Assert.IsTrue(c.HasAttributedMethodBeenCalledOne);
			Assert.IsTrue(c.HasAttributedMethodBeenCalledTwo);

			Assert.IsFalse(c.HasInterfaceImplBeenCalledTwice);
			Assert.IsTrue(c.HasInterfaceImplBeenCalledAfterAllAttribs);
		}
	}
}
