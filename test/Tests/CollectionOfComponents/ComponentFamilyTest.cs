﻿using System.Linq;
using ComposerCore.Implementation;
using ComposerCore.Tests.CollectionOfComponents.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.CollectionOfComponents
{
	[TestClass]
	public class ComponentFamilyTest
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
		public void RegisterSameManyTimesWithoutName()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));

			var cs = _context.GetComponentFamily<ISampleContract>();
			Assert.IsNotNull(cs);
			Assert.AreEqual(3, cs.Count());

			var ca = cs.ToArray();
			Assert.IsNotNull(ca[0]);
			Assert.IsNotNull(ca[1]);
			Assert.IsNotNull(ca[2]);
			Assert.AreNotSame(ca[0], ca[1]);
			Assert.AreNotSame(ca[0], ca[2]);
			Assert.AreNotSame(ca[1], ca[2]);
		}

		[TestMethod]
		public void RegisterSameManyTimesWithName()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentOne));

			var cs = _context.GetComponentFamily<ISampleContract>();
			Assert.IsNotNull(cs);
			Assert.AreEqual(5, cs.Count());

			var ca = cs.ToArray();
			Assert.IsNotNull(ca[0]);
			Assert.IsNotNull(ca[1]);
			Assert.IsNotNull(ca[2]);
			Assert.IsNotNull(ca[3]);
			Assert.IsNotNull(ca[4]);
			Assert.AreNotSame(ca[0], ca[1]);
			Assert.AreNotSame(ca[0], ca[2]);
			Assert.AreNotSame(ca[0], ca[3]);
			Assert.AreNotSame(ca[0], ca[4]);
			Assert.AreNotSame(ca[1], ca[2]);
			Assert.AreNotSame(ca[1], ca[3]);
			Assert.AreNotSame(ca[1], ca[4]);
			Assert.AreNotSame(ca[2], ca[3]);
			Assert.AreNotSame(ca[2], ca[4]);
			Assert.AreNotSame(ca[3], ca[4]);
		}

		[TestMethod]
		public void RegisterDifferentSingleEachGenericQuery()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily<ISampleContract>();
			Assert.IsNotNull(cs);
			Assert.AreEqual(2, cs.Count());

			Assert.AreEqual(1, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(1, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void RegisterDifferentSingleEach()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily(typeof(ISampleContract));
			Assert.IsNotNull(cs);
			Assert.AreEqual(2, cs.Count());

			Assert.AreEqual(1, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(1, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void RegisterDifferentSingleEachGenericQueryWithName()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily<ISampleContract>();
			Assert.IsNotNull(cs);
			Assert.AreEqual(4, cs.Count());

			Assert.AreEqual(2, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(2, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void RegisterDifferentSingleEachWithName()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily(typeof(ISampleContract));
			Assert.IsNotNull(cs);
			Assert.AreEqual(4, cs.Count());

			Assert.AreEqual(2, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(2, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void RegisterDifferentMultipleEachGenericQuery()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register(typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily<ISampleContract>();
			Assert.IsNotNull(cs);
			Assert.AreEqual(5, cs.Count());

			Assert.AreEqual(3, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(2, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void RegisterDifferentMultipleEach()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register(typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily(typeof(ISampleContract));
			Assert.IsNotNull(cs);
			Assert.AreEqual(5, cs.Count());

			Assert.AreEqual(3, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(2, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void RegisterDifferentMultipleEachGenericQueryWithName()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentTwo));
			_context.Register("name", typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily<ISampleContract>();
			Assert.IsNotNull(cs);
			Assert.AreEqual(7, cs.Count());

			Assert.AreEqual(4, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(3, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void RegisterDifferentMultipleEachWithName()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentOne));
			_context.Register("name", typeof(SampleComponentTwo));
			_context.Register("name", typeof(SampleComponentTwo));

			var cs = _context.GetComponentFamily(typeof(ISampleContract));
			Assert.IsNotNull(cs);
			Assert.AreEqual(7, cs.Count());

			Assert.AreEqual(4, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(3, cs.Count(o => o is SampleComponentTwo));
		}

		[TestMethod]
		public void EmptyGetComponentFamilyGeneric()
		{
			var cs = _context.GetComponentFamily<ISampleContract>();

			Assert.IsNotNull(cs);
			Assert.AreEqual(0, cs.Count());
		}

		[TestMethod]
		public void EmptyGetComponentFamily()
		{
			var cs = _context.GetComponentFamily(typeof(ISampleContract));

			Assert.IsNotNull(cs);
			Assert.AreEqual(0, cs.Count());
		}
	}
}
