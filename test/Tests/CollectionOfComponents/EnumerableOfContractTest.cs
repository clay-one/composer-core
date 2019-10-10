using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using ComposerCore.Implementation;
using ComposerCore.Tests.CollectionOfComponents.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.CollectionOfComponents
{
    [TestClass]
    [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
    public class EnumerableOfContractTest
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
            _context.Configuration.DisableAttributeChecking = true;
        }

        [TestCleanup]
        public void TestCleanup()
        {
        }

        #endregion

		[TestMethod]
		public void RuntimeTypeIsCastToContract()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register(typeof(SampleComponentTwo));

			var cs = _context.GetAllComponents(typeof(ISampleContract)) as IEnumerable<ISampleContract>;
			Assert.IsNotNull(cs);
			Assert.AreEqual(5, cs.Count());

			Assert.AreEqual(3, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(2, cs.Count(o => o is SampleComponentTwo));
		}
		
		[TestMethod]
		public void GetComponentWithEnumerableContract()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register(typeof(SampleComponentTwo));

			var cs = _context.GetComponent(typeof(IEnumerable<ISampleContract>)) as IEnumerable<ISampleContract>;
			Assert.IsNotNull(cs);
			Assert.AreEqual(5, cs.Count());

			Assert.AreEqual(3, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(2, cs.Count(o => o is SampleComponentTwo));
		}
		
		[TestMethod]
		public void GetComponentGenericWithEnumerableContract()
		{
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentOne));
			_context.Register(typeof(SampleComponentTwo));
			_context.Register(typeof(SampleComponentTwo));

			var cs = _context.GetComponent<IEnumerable<ISampleContract>>();
			Assert.IsNotNull(cs);
			Assert.AreEqual(5, cs.Count());

			Assert.AreEqual(3, cs.Count(o => o is SampleComponentOne));
			Assert.AreEqual(2, cs.Count(o => o is SampleComponentTwo));
		}
		
		[TestMethod]
		public void RegisterEnumerableDirectly()
		{
			var list = new List<ISampleContract> {new SampleComponentOne()};
			_context.RegisterObject(typeof(IEnumerable<ISampleContract>), list);

			Assert.IsFalse(_context.IsResolvable(typeof(ISampleContract)));
			Assert.IsTrue(_context.IsResolvable<IEnumerable<ISampleContract>>());

			var c = _context.GetComponent<IEnumerable<ISampleContract>>();
			Assert.IsNotNull(c);
			Assert.AreSame(list, c);
		}

		[TestMethod]
		public void DirectEnumerableOverridesIndividualComponents()
		{
			var list = new List<ISampleContract> {new SampleComponentOne()};
			_context.RegisterObject(typeof(IEnumerable<ISampleContract>), list);
			_context.Register(typeof(SampleComponentTwo));

			var c = _context.GetComponent<ISampleContract>();
			Assert.IsNotNull(c);
			Assert.IsTrue(c is SampleComponentTwo);

			var cList = _context.GetComponent<IEnumerable<ISampleContract>>();
			Assert.IsNotNull(cList);
			Assert.AreSame(list, cList);
			Assert.IsTrue(list.First() is SampleComponentOne);

			var cAll = _context.GetAllComponents<ISampleContract>();
			Assert.IsNotNull(cAll);
			Assert.IsTrue(cAll.Count() == 1);
			Assert.IsTrue(cAll.First() is SampleComponentTwo);
		}

		[TestMethod]
		public void EnumerableOfContractsAreAlwaysResolvable()
		{
			// Call without registering any components
			
			Assert.IsTrue(_context.IsResolvable<IEnumerable<ISampleContract>>());
		}

		[TestMethod]
		public void EnumerableElementTypeIsCheckedToBeContract()
		{
			_context.Configuration.DisableAttributeChecking = false;
			
			Assert.IsTrue(_context.IsResolvable<IEnumerable<ISampleContract>>());
			Assert.IsFalse(_context.IsResolvable<IEnumerable<string>>());
			
			_context.Configuration.DisableAttributeChecking = true;
			
			Assert.IsTrue(_context.IsResolvable<IEnumerable<ISampleContract>>());
			Assert.IsTrue(_context.IsResolvable<IEnumerable<string>>());
		}
    }
}