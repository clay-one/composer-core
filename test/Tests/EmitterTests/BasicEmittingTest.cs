﻿using System;
using System.Reflection.Emit;
using ComposerCore.Aop.Emitter;
using ComposerCore.Aop.Utility;
using ComposerCore.Attributes;
using ComposerCore.Implementation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ComposerCore.Tests.EmitterTests.Components;
using ComposerCore.Utility;

namespace ComposerCore.Tests.EmitterTests
{
	[TestClass]
	public class BasicEmittingTest
	{
		public TestContext TestContext { get; set; }
		private ComponentContext _context;
		private IClassEmitter _classEmitter;
		private TestingEmittedTypeHandler _dth;

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
			_context.RegisterAssembly(typeof(Aop.AssemblyPointer).Assembly);
			_classEmitter = _context.GetComponent<IClassEmitter>();

//			UNCOMMENT FOLLOWING FOR DEBUGGING
//			_classEmitter = new DefaultClassEmitter {SaveEmittedAssembly = true, SaveTargetFolder = "D:\\"};
//			_context.InitializePlugs(_classEmitter, typeof (DefaultClassEmitter));

			_dth = new TestingEmittedTypeHandler();
		}

		[TestCleanup]
		public void TestCleanup()
		{
		}

		#endregion

		[TestMethod]
		public void EmptyInterface()
		{
			var di = _classEmitter.EmitInterfaceInstance<IEmpty>(_dth);

			Assert.IsNotNull(di);
		}

		[TestMethod]
		public void EmptyInterfaceWithAdditionalAttributes()
		{

			var attributeBuilder = new CustomAttributeBuilder(
				typeof (ComponentAttribute).GetConstructor(Type.EmptyTypes),
				new object[0]);

			var di = _classEmitter.EmitInterfaceInstance<IEmpty>(_dth, null, null, new[] { attributeBuilder });

			Assert.IsNotNull(di);
			Assert.IsTrue(ComponentContextUtils.HasComponentAttribute(di.GetType()));
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AbstractClassThrows()
		{
			_classEmitter.EmitInterfaceInstance<SomeAbstractClass>(_dth);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void ConcreteClassThrows()
		{
			_classEmitter.EmitInterfaceInstance<SomeConcreteClass>(_dth);
		}
	}
}
