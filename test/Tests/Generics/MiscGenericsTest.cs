﻿using ComposerCore.Implementation;
using ComposerCore.Tests.Generics.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.Generics
{
	[TestClass]
	public class MiscGenericsTest
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
		[ExpectedException(typeof(CompositionException))]
		public void QueryWithOpenGenericContractFails()
		{
			_context.Register(typeof(OpenComponentOne<>));
			_context.GetComponent(typeof (IGenericContractOne<>));
		}
		 
	}
}