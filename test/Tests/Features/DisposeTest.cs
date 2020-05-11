using System;
using System.Linq;
using ComposerCore.Implementation;
using ComposerCore.Tests.Features.Components;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComposerCore.Tests.Features
{
    [TestClass]
    public class DisposeTest
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
        public void DisposeSingletonComponent()
        {
            _context.Register<DisposableSingletonComponent>();

            var c = _context.GetComponent<DisposableSingletonComponent>();
            
            _context.Dispose();
            
            Assert.IsTrue(c.Disposed);
        }

        [TestMethod]
        public void DisposeTransientComponent()
        {
            _context.Register<DisposableTransientComponent>();

            var c = _context.GetComponent<DisposableTransientComponent>();
            
            _context.Dispose();
            
            Assert.IsTrue(c.Disposed);
        }

        [TestMethod]
        public void TransientIsDisposedWithGc()
        {
            void CreateComponentLocalFunction()
            {
                _context.GetComponent<DisposableTransientComponent>();
            }

            _context.Register<DisposableTransientComponent>();
            CreateComponentLocalFunction();
            
            Assert.AreEqual(1, _context.Repository.RecycleBin.Count(wr => wr.TryGetTarget(out _)));
            
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            
            Assert.AreEqual(0, _context.Repository.RecycleBin.Count(wr => wr.TryGetTarget(out _)));
        }

        [TestMethod]
        public void NonDisposableIsNotKept()
        {
            _context.Register<NonDisposableSingletonComponent>();
            _context.Register<NonDisposableTransientComponent>();

            _context.GetComponent<NonDisposableSingletonComponent>();
            _context.GetComponent<NonDisposableTransientComponent>();
            
            Assert.IsFalse(_context.Repository.RecycleBin.Any());
        }

        [TestMethod]
        public void UnregisterDoesNotDispose()
        {
            _context.Register<DisposableSingletonComponent>();
            _context.Register<DisposableTransientComponent>();

            var c1 = _context.GetComponent<DisposableSingletonComponent>();
            var c2 = _context.GetComponent<DisposableTransientComponent>();
            
            _context.Unregister(typeof(DisposableSingletonComponent));
            _context.Unregister(typeof(DisposableTransientComponent));
            
            Assert.IsFalse(c1.Disposed);
            Assert.IsFalse(c2.Disposed);
        }

        [TestMethod]
        public void UnregisterThenDispose()
        {
            _context.Register<DisposableSingletonComponent>();
            _context.Register<DisposableTransientComponent>();

            var c1 = _context.GetComponent<DisposableSingletonComponent>();
            var c2 = _context.GetComponent<DisposableTransientComponent>();
            
            _context.Unregister(typeof(DisposableSingletonComponent));
            _context.Unregister(typeof(DisposableTransientComponent));
            _context.Dispose();
            
            Assert.IsTrue(c1.Disposed);
            Assert.IsTrue(c2.Disposed);
        }
        
        [TestMethod]
        public void RegisterAfterDispose()
        {
            _context.Dispose();
            Assert.ThrowsException<InvalidOperationException>(() => _context.Register<DisposableTransientComponent>());
        }

        [TestMethod]
        public void QueryAfterDispose()
        {
            _context.Dispose();
            Assert.ThrowsException<InvalidOperationException>(() => _context.GetComponent<DisposableTransientComponent>());
        }

        [TestMethod]
        public void SetVariableAfterDispose()
        {
            _context.Dispose();
            Assert.ThrowsException<InvalidOperationException>(() => _context.SetVariableValue("name", new object()));
        }

        [TestMethod]
        public void GetVariableAfterDispose()
        {
            _context.SetVariableValue("name", new object());
            _context.Dispose();
            
            Assert.ThrowsException<InvalidOperationException>(() => _context.GetVariable("name"));
        }

        [TestMethod]
        public void CreateScopeAfterDispose()
        {
            _context.Dispose();
            Assert.ThrowsException<InvalidOperationException>(() => _context.CreateScope());
        }

        [TestMethod]
        public void CreateChildContextAfterDispose()
        {
            _context.Dispose();
            Assert.ThrowsException<InvalidOperationException>(() => _context.CreateChildContext());
        }
    }
}